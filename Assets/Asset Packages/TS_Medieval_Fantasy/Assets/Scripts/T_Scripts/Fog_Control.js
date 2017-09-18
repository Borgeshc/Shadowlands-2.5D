// Fog_Control : Description : Use to change fog using triggers.
#pragma strict
@Header ("Connect the GameObject associated with this GameObject")
var otherTrigger 						: GameObject;			// Connect the gameObject associated with this object.
private var _script						: Fog_Control;			// access component
@Header ("Transition Duration")
var param_Duration 						: float = .5;			// the duration of the transition
@Header ("Color")	
var param_Color 						: Color = Color(1,0,0); // The color you want 
@Header ("Linear")
var param_Start	 						: float = 0;			// The start fog distance you want
var param_End	 						: float = 300;			// The end fog distance you want

private var b_timer						: boolean = false;		// Those variabes are used to create the transition
private var startColor 					: Color;
private var endColor 					: Color;
private var target_fogStartDistance_01	: float;
private var target_fogStartDistance_02	: float;
private var target_fogEndDistance_01	: float;
private var target_fogEndDistance_02	: float;
private var duration					: float = 1; 					
private var t							: float = 0; 					 

function Start () {														// --> Start : Initialisation
	_script =  otherTrigger.GetComponent.<Fog_Control>();				// access Component
}

function Update () {													// --> Update
	if(b_timer){													
		RenderSettings.fogColor = Color.Lerp(startColor, endColor, t);	// Change fogColor
		RenderSettings.fogStartDistance = 								// Change fogStartDistance
			Mathf.Lerp(target_fogStartDistance_01, 
						target_fogStartDistance_02, t);
		RenderSettings.fogEndDistance = 								// Change fogEndDistance
			Mathf.Lerp(target_fogEndDistance_01, 
						target_fogEndDistance_02, t);
    	if (t < 1){
	    	t += Time.deltaTime/duration;
	    }
	    else{
	    	b_timer = false;
	    }
	}
}

function ChangeFogParam(Fog_Start : float,Fog_End : float,FogColor_End : Color, duration_ : float){	//--> Mode == FogMode.ExponentialSquared || Mode == FogMode.Exponential
	target_fogStartDistance_01 	= RenderSettings.fogStartDistance;
	target_fogStartDistance_02	= Fog_Start;
	target_fogEndDistance_01 	= RenderSettings.fogEndDistance;
	target_fogEndDistance_02 	= Fog_End;
	startColor = RenderSettings.fogColor;
	endColor = FogColor_End;
	duration = duration_;
	t = 0;
	b_timer = true;
}

function OnTriggerEnter(other : Collider){									// --> When player enter the trigger
	if(other.tag == "Player"){
		ChangeFogParam(param_Start,param_End,param_Color,param_Duration);	// Call ChangeFogParam
		_script.Stop();														// Stop the transition of otherTrigger if needed
	}
}


function Stop(){b_timer = false;}											// --> Use to stop transition