using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class storyPlayer : MonoBehaviour {
	Text textContent;
	RawImage imageContent;
	RawImage textArrow;
	Text skipMsg;
	RawImage fadeFX;

	float timeSpan;
	public float typingSpan = 10f;
	public float blinkSpan = 0.5f;
	public float imageApreciationSpan = 1.5f;

	public int maxCharsPerLine = 10;
	public string texto;
	float startedTime;
	int text_i=0;
	bool finishedLine;
	bool finishedChapter=false;
	int chapter_i;
	int image_i;
	bool imageTransition;

	int textOffset=0;
	bool canSkip=false;
	public float deltaSkip=0.5f;

	//fading effect
	bool blackin;
	bool blackout;

	public string[] textChapters;
	public Texture[] frameImages;
	public int[] imageChapters;

	public GameObject nextShort;
	public GameObject leftOptionShort;
	public GameObject middleOptionShort;
	public GameObject rightOptionShort;

	public string nextSceneName;
	public string leftOptionSceneName;
	public string middleOptionSceneName;
	public string rightOptionSceneName;

	GameObject eventSys;
	GameObject rightOpt;
	GameObject midOpt;
	GameObject leftOpt;

	AudioSource myAudio;
	public AudioClip leftButtonSound;
	public AudioClip middleButtonSound;
	public AudioClip rightButtonSound;

	bool leftOption;
	bool middleOption;
	bool rightOption;

	string spaceCode = " ";

	// Use this for initialization
	void Start () {
		eventSys = GameObject.Find(gameObject.name + "/EventSystem");
		rightOpt = GameObject.Find(gameObject.name + "/rightOption");
		midOpt = GameObject.Find(gameObject.name + "/middleOption");
		leftOpt = GameObject.Find(gameObject.name + "/leftOption");
		EnableOptions (false);

		textContent = GameObject.Find (gameObject.name + "/contentText").GetComponent<Text> ();
		imageContent = GameObject.Find (gameObject.name + "/content").GetComponent<RawImage> ();
		textArrow = GameObject.Find (gameObject.name + "/contentArrow").GetComponent<RawImage> ();
		skipMsg = GameObject.Find (gameObject.name + "/skipText").GetComponent<Text> ();
		skipMsg.enabled = false;
		fadeFX = GameObject.Find (gameObject.name + "/fadeFX").GetComponent<RawImage> ();
		fadeFX.CrossFadeAlpha(0,0,true);
		textArrow.CrossFadeAlpha (0, 0, true);
		textContent.text = "";
		startedTime = Time.time;
		timeSpan = typingSpan;
		texto = textChapters[chapter_i];
		Debug.Log ("length of current text: "+texto.Length);

		myAudio = GetComponent<AudioSource> ();
		if (frameImages.Length != imageChapters.Length) {
			Time.timeScale = 0;
			Debug.Log ("Error! The frameImages and imageChapters array must have the same length, one specifies when/where to display the images added to the other");
		}

		//available options
		leftOption = leftOptionShort!=null;
		rightOption = rightOptionShort!=null;
		middleOption = middleOptionShort!=null;
		Debug.Log ("current res: "+Screen.width.ToString() + " per "+ Screen.height.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		if (imageTransition) {
			if (Time.time - startedTime >= timeSpan) {
				if (blackin) {
					updateImage();
				}
				else {
					if (blackout) {
						//appreciating
						startedTime = Time.time;
						timeSpan = imageApreciationSpan;
						blackout = false;
					}
					else{
						//blackout, release to go back to the text in case there is no followup image
						if (image_i < frameImages.Length) { 
							if (imageChapters [image_i] == chapter_i + 1) {
								//fade image again
								FadeINimage ();
							} else {
								//return to texts
								if (chapter_i < textChapters.Length - 1)
									NextChapterText ();
								else
									NextEvent ();
							}
						} else {
							//return to texts
							if (chapter_i < textChapters.Length - 1)
								NextChapterText ();
							else
								NextEvent ();
						}
					}
				}
				//startedTime = Time.time;
			}						
		} 
		else {			
			skipMsg.enabled = canSkip;

			if (Input.GetKeyDown ("down") || Input.GetKeyDown(KeyCode.Return) ) {
				//Debug.Log ("key pressed");
				if (canSkip || finishedLine) {				
					textOffset += text_i;
					if (textOffset < texto.Length) {
						resetLine ();
					} else {
						if (chapter_i < textChapters.Length - 1 || image_i < frameImages.Length - 1) {
							if(image_i < frameImages.Length){ 
								if (imageChapters [image_i] == chapter_i + 1) {
									FadeINimage ();
								} else {
									NextChapterText ();
								}
							} 
							else {
								NextChapterText ();
							}
						} else {
							NextEvent ();
						}
					}
				} else {
					StartCoroutine (mightSkip ());
				}
			}
			if (Time.time - startedTime >= timeSpan) {
				if (text_i < maxCharsPerLine && text_i + textOffset < texto.Length && WordPredictor()) {
					textContent.text += texto [text_i + textOffset];
					text_i += 1;
				} else {
					timeSpan = blinkSpan;
					textArrow.CrossFadeAlpha (0, 0, true);
					finishedLine = true;
					if (!finishedChapter)
						textArrow.CrossFadeAlpha (1, timeSpan, false);
				}
				startedTime = Time.time;
			}
		}
	}
	bool WordPredictor(){		
		int aux_i = text_i;
		while (aux_i < maxCharsPerLine && aux_i + textOffset< texto.Length) {
			if (texto [aux_i + textOffset] == spaceCode[0])
				return true;
			aux_i += 1;
		}
		if (aux_i + textOffset >= texto.Length)
			return true;
		else
			return false;
	}
	void FadeINimage(){
		timeSpan = blinkSpan;
		imageTransition = true;
		blackin = true;
		skipMsg.enabled = false;
		canSkip = false;
		textArrow.CrossFadeAlpha (0, 0, true);
		fadeFX.CrossFadeAlpha(1,timeSpan,false);
		startedTime = Time.time;
	}
	void updateImage(){
		textContent.text = "";
		imageContent.texture = frameImages [image_i];
		image_i += 1;
		startedTime = Time.time;
		blackin = false;
		blackout = true;
		fadeFX.CrossFadeAlpha(0,timeSpan,false);
	}
	void resetLine(){
		text_i = 0;
		textContent.text = "";
		timeSpan = typingSpan;
		textArrow.CrossFadeAlpha (0, 0, true);
		finishedLine = false;
	}
	void NextChapterText(){
		imageTransition = false;
		chapter_i += 1;
		texto = textChapters [chapter_i];
		textOffset = 0;
		resetLine ();
	}
	void NextEvent(){
		//check if there is a scene or a prefab without user choice
		if (nextShort != null) {
			GameObject.Instantiate(nextShort, transform.position,transform.rotation);
			Destroy (gameObject);
		} 
		else {
			if (nextSceneName != "") {
				SceneManager.LoadScene (nextSceneName);
			}
			else {
				//otherwise turn on the options
				imageTransition = false;
				textContent.text = "";
				textArrow.CrossFadeAlpha (0, 0, true);
				EnableOptions (true);
				finishedChapter = true;
			}
		}
		Debug.Log ("Load next phase here!");
	}

	IEnumerator mightSkip() {
		canSkip = true;
		while (text_i < maxCharsPerLine && text_i+textOffset < texto.Length && WordPredictor()) {
			textContent.text += texto [text_i+textOffset];
			text_i += 1;
		}
		yield return new WaitForSeconds (deltaSkip);
		canSkip = false;
	}
	void EnableOptions(bool enableGuys){
		eventSys.SetActive (enableGuys);

		rightOpt.SetActive (enableGuys && rightOption);
		midOpt.SetActive (enableGuys && middleOption);
		leftOpt.SetActive (enableGuys && leftOption);
	}
	public void RightOption(){
		myAudio.PlayOneShot(rightButtonSound);
		Debug.Log ("right option selected");
		if (rightOptionShort != null) {
			GameObject.Instantiate (rightOptionShort, transform.position, transform.rotation);
			Destroy (gameObject);
		} else {
			if (rightOptionSceneName != "") {
				SceneManager.LoadScene (rightOptionSceneName);
			} else {
				Debug.Log ("Error! Neither a prefab nor a scene name were specified for the right option in the end of the short.");
			}
		}
	}
	public void MiddleOption(){
		Debug.Log ("middle option selected");
		myAudio.PlayOneShot(middleButtonSound);
		if (middleOptionShort != null) {
			GameObject.Instantiate (middleOptionShort, transform.position, transform.rotation);
			Destroy (gameObject);
		} else {
			if (middleOptionSceneName != "") {
				SceneManager.LoadScene (middleOptionSceneName);
			} else {
				Debug.Log ("Error! Neither a prefab nor a scene name were specified for the middle option in the end of the short.");
			}
		}
	}
	public void LeftOption(){
		Debug.Log ("left option selected");
		myAudio.PlayOneShot(leftButtonSound);
		if (leftOptionShort != null) {
			GameObject.Instantiate (leftOptionShort, transform.position, transform.rotation);
			Destroy (gameObject);
		} else {
			if (leftOptionSceneName != "") {
				SceneManager.LoadScene (leftOptionSceneName);
			} else {
				Debug.Log ("Error! Neither a prefab nor a scene name were specified for the left option in the end of the short.");
			}
		}
	}
}
