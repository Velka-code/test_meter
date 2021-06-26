using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class dataGeneretor : MonoBehaviour
{
        
    public float LowValve;
    public float LowEvaporator;
    public float LowCompressor;
    public float HighValve;
    public float HighEvaporator;
    public float HighCompressor;

    public float Temperature;
    public bool dummy = true;

    public GameObject ble;

       
    // Start is called before the first frame update
    public void Start()
    {
        this.ble = GameObject.Find("BLE");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Pointer_value()
    {
        StartCoroutine(DelayCoroutine());

    }

    private IEnumerator DelayCoroutine()
    {
        dummy = false;

        
        //圧力計6+温度データをダミーで生成
        float d1 = Random.Range(0.5f, 1.0f);
        float d2 = Random.Range(1.0f, 1.5f);
        float d3 = Random.Range(2.0f, 2.5f);
        float d4 = Random.Range(2.5f, 3.0f);
        float d5 = Random.Range(3.0f, 3.5f);
        float d6 = Random.Range(1.0f, 1.5f);
        float d7 = Random.Range(-5.0f, 3.0f); 

        
        //小数点以下1位までのデータに丸め
        d1 = d1 * 10;
        this.LowValve = Mathf.Floor(d1) / 10;
        
        d2 = d2 * 10;
        this.LowEvaporator = Mathf.Floor(d2) / 10; 
        
        d3 = d3 * 10;
        this.LowCompressor = Mathf.Floor(d3) / 10;
         
        d4 = d4 * 10;
        this.HighValve = Mathf.Floor(d4) / 10;
        
        d5 = d5 * 10;
        this.HighEvaporator = Mathf.Floor(d5) / 10;
        
        d6 = d6 * 10;
        this.HighCompressor = Mathf.Floor(d6) / 10;

        d7 = d7 * 10;
        this.Temperature = Mathf.Floor(d7) / 10;

        //Debug.Log("LowValve: " + this.LowValve);
        //Debug.Log("LowEvaporator: " + this.LowEvaporator);
        //Debug.Log("LowCompressor: " + this.LowCompressor);

        //Debug.Log("HighValve: " + this.HighValve);
        //Debug.Log("HighEvaporator: " + this.HighEvaporator);
        //Debug.Log("HighCompressor: " + this.HighCompressor);

        //Debug.Log("Temperature: " + this.Temperature);

        /*

        //BLE通信によりデータを取得
        this.LowValve = this.ble.GetComponent<BLEManagerG>().LowValveData;
        this.LowEvaporator = this.ble.GetComponent<BLEManagerF>().LowEvaporatorData;        
        this.LowCompressor = this.ble.GetComponent<BLEManagerE>().LowCompressorData;
        this.HighValve = this.ble.GetComponent<BLEManagerD>().HighValveData;
        this.HighEvaporator = this.ble.GetComponent<BLEManagerC>().HighEvaporatorData;
        this.HighCompressor = this.ble.GetComponent<BLEManagerB>().HighCompressorData;        
        this.Temperature = this.ble.GetComponent<BLEManager>().TemperatureData;
        */

        // 1秒間待つ
        yield return new WaitForSeconds(1);


        dummy = true;
    }
}
