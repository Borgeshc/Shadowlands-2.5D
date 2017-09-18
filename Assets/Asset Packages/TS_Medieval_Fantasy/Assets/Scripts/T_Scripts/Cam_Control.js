// Cam_Control : Description : Use to change distance between player and camera using triggers.
#pragma strict
var Cam 								: GameObject;
private var cam 						: followPlayer;
private var Cam_Offset 					: Vector3;
var New_Offset_Y						: float;
var New_Offset_Z						: float;
@Header ("Connect the GameObject associated with this GameObject")
var otherTrigger 						: GameObject;					// Connect the gameObject associated with this object.
private var _script						: Cam_Control;					// access component
@Header ("Transition Duration")
var param_Duration 						: float = .5;					// the duration of the transition


private var b_timer						: boolean = false;				// Those variabes are used to create the transition
private var target_Distance_01_Z		: float;
private var target_Distance_02_Z		: float;
private var target_Distance_01_Y		: float;
private var target_Distance_02_Y		: float;
private var duration					: float = 1; 					
private var t							: float = 0; 					 

function Start () {														// --> Start : Initialisation
	if(otherTrigger)
	_script =  otherTrigger.GetComponent.<Cam_Control>();				// access Component
	if(Cam == null)
  	Cam = GameObject.FindWithTag("MainCamera");
		cam = Cam.GetComponent.<followPlayer>();						// access Component
}

function FixedUpdate () {												// --> Update
	if(b_timer){													
		Cam_Offset.z = 													// Change fogStartDistance
			Mathf.Lerp(target_Distance_01_Z, 
						target_Distance_02_Z, t);

		Cam_Offset.y = 													// Change fogStartDistance
			Mathf.Lerp(target_Distance_01_Y, 
						target_Distance_02_Y, t);

		cam.DistanceZ(Cam_Offset.y,Cam_Offset.z);

    	if (t < 1){
	    	t += Time.deltaTime/duration;
	    }
	    else{
	    	b_timer = false;
	    }
	}
}

function ChangeFogParam(Offset_Y : float, Offset_Z: float, duration_ : float){	//--> Mode == FogMode.ExponentialSquared || Mode == FogMode.Exponential
	target_Distance_01_Z 	= cam.Return_DistanceZ();
	target_Distance_02_Z	= Offset_Z;

	target_Distance_01_Y 	= cam.Return_DistanceY();
	target_Distance_02_Y	= Offset_Y;

	duration = duration_;
	t = 0;
	b_timer = true;
}

function OnTriggerEnter(other : Collider){									// --> When player enter the trigger
	if(other.tag == "Player" && otherTrigger){
		ChangeFogParam(New_Offset_Y,New_Offset_Z,param_Duration);
		_script.Stop();														// Stop the transition of otherTrigger if needed
	}
}


function Stop(){b_timer = false;}											// --> Use to stop transition