# Face-Detect-With-Unity3D
Face-Detect-With-Unity3D

>最近也在写关于图像识别的算法,主要是自己在做增强现实的项目,和图形图像都一直打交道.然后有个朋友刚好人脸识别那块遇到瓶颈,刚好我项目当中也碰到一个相关的问题,就顺便一起解决
>
>用Unity3d的处理图像,或多或少都会接触OpenCV这个库,百度文库上也有一篇是关于基于SIFT关键点的增强现实初始化算法
http://wenku.baidu.com/view/10589dc108a1284ac8504390.html
>
具体是处理增强现实技术那一块的东西的,我们所谓的AR,其实本质上也是扫描图像的特征点,然后识别出一个特征数组,然后和预先存好的数据进行比较,如果相识的就认为它们是同一个物体.其实这个在淘宝的拍立淘项目也有用上
>
本质上做人脸识别\商品识别\LOGO识别都一样的,OpenCV大法的确很好,现在第三方实现OpenCV库的资料很多,python和node.js实现云识别的项目也在Github也能找到,那我这次实现的也依赖于这些第三方库,以及Unity3D这个引擎.

先上个Unity工程的截图,黑一下钟汉良.

![Paste_Image.png](http://upload-images.jianshu.io/upload_images/442033-8a213202c13a0206.png?imageMogr2/auto-orient/strip%7CimageView2/2/w/1240)

然后公开Unity3D的源代码

```
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
```
