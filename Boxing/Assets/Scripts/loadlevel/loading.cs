using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class loading : MonoBehaviour {

    private float fps = 10.0f;
    private float time;
    //一组动画的贴图，在编辑器中赋值；
    public Texture2D[] animations;
    private int nowFram;

    //异步对象
    AsyncOperation async;

    //读取场景进度，他的取值范围在0-1之间；
    int progress = 0;
	// Use this for initialization
	void Start () {
        //开启一个异步任务
        //进入loadscene的方法
        StartCoroutine(loadscene());
	}
	
    IEnumerator loadscene()
    {
        //异步读取场景
        //global.loadname 就是A场景中需要读取的c场景名。
        async = SceneManager.LoadSceneAsync(global.loadName);

        //读取完毕后会自动进入c场景；
        yield return async;
    }

	// Update is called once per frame
	void Update () {
        //在这里计算读取的进度
        //progress的取值范围在0.1-1之间，但是他不会等于
        //在progress等于0.9的时候就直接进入新的场景
        //所以在协警读条的时候需要注意
        //为了计算百分比，所以直接乘以100即可
        progress = (int)(async.progress * 100);
        Debug.Log(global.gametime+"进度" + progress);
        
	}
}
