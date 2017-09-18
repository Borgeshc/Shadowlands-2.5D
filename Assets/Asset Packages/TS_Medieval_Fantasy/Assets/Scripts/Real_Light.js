// Description : Real_Light.js : This script randomly generate the intensity and the color of a light
#pragma strict

var minIntensity 		: float = 0.25f;					// minimum intensity
var maxIntensity		: float = 0.5f;						// Maximum intensity
 
private var rand 		: float;  		


private var _light		: Light;
var color_01			: Color = Color(1,.5,.2);			// Choose First color
var color_02			: Color = Color(1,.8,.2);			// Choose Second color
var speed 				: float = 4;								

function Start(){
	_light =  GetComponent.<Light>();									
	rand = Random.Range(0.0f, 65535.0f);
}
 
function Update(){
	var noise = Mathf.PerlinNoise(rand, Time.time*speed);
	_light.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);	// Change intensity
	_light.color = Color.Lerp(color_01, color_02, noise);				// Change color
}