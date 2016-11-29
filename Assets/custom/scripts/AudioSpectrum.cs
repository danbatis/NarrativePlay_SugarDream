using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioListener))]
public class AudioSpectrum : MonoBehaviour
{
	public float[] spectrum = new float[256];

	void Update( )
	{
		AudioListener.GetSpectrumData( spectrum, 0, FFTWindow.BlackmanHarris );

		for( int i = 1; i < spectrum.Length-1; i++ )
		{
			Debug.DrawLine( new Vector3( i - 1, spectrum[i] + 10, 0 ), new Vector3( i, spectrum[i + 1] + 10, 0 ), Color.red );
			Debug.DrawLine( new Vector3( i - 1, Mathf.Log( spectrum[i - 1] ) + 10, 2 ), new Vector3( i, Mathf.Log( spectrum[i] ) + 10, 2 ), Color.cyan );
			Debug.DrawLine( new Vector3( Mathf.Log( i - 1 ), spectrum[i - 1] - 10, 1 ), new Vector3( Mathf.Log( i ), spectrum[i] - 10, 1 ), Color.green );
			Debug.DrawLine( new Vector3( Mathf.Log( i - 1 ), Mathf.Log( spectrum[i - 1] ), 3 ), new Vector3( Mathf.Log( i ), Mathf.Log( spectrum[i] ), 3 ), Color.blue );

			//Now something resembling audio bars of the old soundsystems:
			Vector3 barPos = new Vector3( transform.position.x+i, transform.position.y, transform.position.z );
			Vector3 barEnd = new Vector3( transform.position.x+i, transform.position.y+Mathf.Log(spectrum[i])*(-10), transform.position.z );
			Debug.DrawLine( barPos,barEnd , Color.red );
		}
	}
}