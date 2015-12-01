using UnityEngine;
using System.Collections;

public class Demo : MonoBehaviour {

	//人脸识别结果的数据结构
	public class face{
		public Facemodels[] faces;		//人脸数组,最大返回的脸为10张
	}
	//每张人脸识别结果的数据结构
	public class Facemodels{
		public int facerectanglex;			//人脸矩形框x坐标
		public int facerectangley;			//人脸矩形框y坐标
		public int facerectanglewidth;		//人脸矩形框宽
		public int facerectangleheigh;		//人脸矩形框高
		public string base64feature;		//人脸的特征值
		public int lefteyex;		//人脸左眼特征坐标
		public int lefteyey;		//人脸左眼特征坐标
		public int righteyex;		//人脸右眼特征坐标
		public int righteyey;		//人脸右眼特征坐标
		public int mouthx;			//人脸口型特征坐标
		public int mouthy;			//人脸口型特征坐标

	}

	public Facemodels people = new Facemodels();

	Texture2D _tex = null;
	string _message = null;

	IEnumerator Start(){

		//加载本地的一张美女图片
		//string filePath = "file://" + Application.dataPath + @"/lady.jpg";
		string filePath = "file://" + Application.dataPath + @"/man.jpg";
		WWW www = new WWW(filePath);
		yield return www ;
		_tex = www.texture;

		//进行人脸识别算法
		CV2FaceDectect cv2 = new CV2FaceDectect();
		StartCoroutine (cv2.IRequestJPG(_tex,Result));
	}

	//接收识别到的结果
	void Result(string message){

		Debug.Log (message);

		LitJson.JsonData _data= LitJson.JsonMapper.ToObject(message);
		LitJson.JsonData _person = _data ["facemodels"];

		Debug.Log ("识别到的人数"+_person.Count);

		for(int i= 0;i<_person.Count;i++){

			people.facerectanglex= (int)_person[i]["facerectanglex"];
			people.facerectangley= (int)_person[i]["facerectangley"];
			people.facerectanglewidth= (int)_person[i]["facerectanglewidth"];
			people.facerectangleheigh= (int)_person[i]["facerectangleheight"];
			people.lefteyex= (int)_person[i]["lefteye"]["x"];
			people.lefteyey= (int)_person[i]["lefteye"]["y"];
			people.righteyex= (int)_person[i]["righteye"]["x"];
			people.righteyey= (int)_person[i]["righteye"]["y"];
			people.mouthx= (int)_person[i]["mouth"]["x"];
			people.mouthy= (int)_person[i]["mouth"]["y"];

			_message = "识别第"+i+"个人,"
			         + "他的脸框坐标x,y是"+(int)_person[i]["facerectanglex"] +"和"+ (int)_person[i]["facerectangley"]
			         + "他的脸框坐标宽,高是"+(int)_person[i]["facerectanglewidth"] +"和"+ (int)_person[i]["facerectangleheight"]
			         + "他的左眼坐标x,y是"+(int)_person[i]["lefteye"]["x"] +"和"+ (int)_person[i]["lefteye"]["y"]
			         + "他的右眼坐标x,y是"+(int)_person[i]["righteye"]["x"] +"和"+ (int)_person[i]["righteye"]["y"]
				     + "他的嘴巴坐标x,y是"+(int)_person[i]["mouth"]["x"] +"和"+ (int)_person[i]["mouth"]["y"]	
				;	          
			
			Debug.Log("信息:"+_message);
		}
	}

	//打印加载的图片到屏幕上
	void OnGUI(){
		if(_tex != null){
			GUI.Box(new Rect(0,0,_tex.width,_tex.height),_tex);
		}
		if(_message != null){
			GUILayout.TextArea(_message);
			//框住脸
			GUI.Box(new Rect(people.facerectanglex,people.facerectangley,people.facerectanglewidth,people.facerectangleheigh),"脸");
			//框住眼
			GUI.Box(new Rect(people.lefteyex-20,people.lefteyey-20,40,40),"左");
			GUI.Box(new Rect(people.righteyex-20,people.righteyey-20,40,40),"右");
			//框住口
			GUI.Box(new Rect(people.mouthx-20,people.mouthy-20,40,40),"口");
		}
	}
}
