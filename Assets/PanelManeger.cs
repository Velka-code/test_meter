using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManeger : MonoBehaviour
{

    private float Lo_Valve_Value;
    private float Hi_Valve_Value;
    private float Lo_Evaporator_Value;
    private float Hi_Evaporator_Value;
    private float Lo_Compressor_Value;
    private float Hi_Compressor_Value;
    private int Time_Minute_Value;
    
    public int separateTime;

    private const float Lo_Valve_Max = 2.0f;
    private const float Lo_Valve_Min = 0.0f;
    private const float Hi_Valve_Max = 4.0f;
    private const float Hi_Valve_Min = 0.0f;
    private const float Lo_Evaporator_Max = 2.0f;
    private const float Lo_Evaporator_Min = 0.0f;
    private const float Hi_Evaporator_Max = 4.0f;
    private const float Hi_Evaporator_Min = 0.0f;
    private const float Lo_Compressor_Max = 4.0f;
    private const float Lo_Compressor_Min = 0.0f;
    private const float Hi_Compressor_Max = 6.0f;
    private const float Hi_Compressor_Min = 0.0f;
    private const int Time_Minute_Max = 10;
    private const int Time_Minute_Min = 1;

    private InputField input_Lo_Valve_value;
    private InputField input_Lo_Evaporator_value;
    private InputField input_Lo_Compressor_value;
    private InputField input_Hi_Valve_value;
    private InputField input_Hi_Evaporator_value;
    private InputField input_Hi_Compressor_value;
    private InputField input_Time_Minute_value;

    
    private Button defultbutton;
    private Button setbutton;
    private Button BLEch1button;
    private Button BLEch2button;
    private Button BLEch3button;
    private Button BLEch4button;
    private Button BLEch5button;
    private Button BLEch6button;
    private Button BLEch7button;

    public GameObject canvas;
    public GameObject ble;

    //public Text Lo_Valve_value;

    // Start is called before the first frame update
    void Start()
    {
        this.input_Lo_Valve_value = GameObject.Find("SysLo_ValveInput").GetComponentInChildren<InputField>();
        this.input_Lo_Evaporator_value = GameObject.Find("SysLo_EvaporatorInput").GetComponentInChildren<InputField>();
        this.input_Lo_Compressor_value = GameObject.Find("SysLo_CompressorInput").GetComponentInChildren<InputField>();
        this.input_Hi_Valve_value = GameObject.Find("SysHi_ValveInput").GetComponentInChildren<InputField>();
        this.input_Hi_Evaporator_value = GameObject.Find("SysHi_EvapotatorInput").GetComponentInChildren<InputField>();
        this.input_Hi_Compressor_value = GameObject.Find("SysHi_CompressorInput").GetComponentInChildren<InputField>();
        this.input_Time_Minute_value = GameObject.Find("Sytime_minuteInput").GetComponentInChildren<InputField>();

        this.input_Lo_Valve_value.text = "2.0";
        this.input_Lo_Evaporator_value.text = "2.0";
        this.input_Lo_Compressor_value.text = "4.0";
        this.input_Hi_Valve_value.text = "4.0";
        this.input_Hi_Evaporator_value.text = "2.0";
        this.input_Hi_Compressor_value.text = "4.0";
        this.input_Time_Minute_value.text = "10";

        //アラーム設定のデフォルト値
        this.Lo_Valve_Value = 2.0f;
        this.Lo_Evaporator_Value = 2.0f;
        this.Lo_Compressor_Value = 4.0f;
        this.Hi_Valve_Value = 4.0f;
        this.Hi_Evaporator_Value = 4.0f;
        this.Hi_Compressor_Value = 6.0f;
        
        this.defultbutton = GameObject.Find("DefultButton").GetComponentInChildren<Button>();
        this.setbutton = GameObject.Find("SetButton").GetComponentInChildren<Button>();
        this.BLEch1button = GameObject.Find("BLEch1Button").GetComponentInChildren<Button>();
        this.BLEch2button = GameObject.Find("BLEch2Button").GetComponentInChildren<Button>();
        this.BLEch3button = GameObject.Find("BLEch3Button").GetComponentInChildren<Button>();
        this.BLEch4button = GameObject.Find("BLEch4Button").GetComponentInChildren<Button>();
        this.BLEch5button = GameObject.Find("BLEch5Button").GetComponentInChildren<Button>();
        this.BLEch6button = GameObject.Find("BLEch6Button").GetComponentInChildren<Button>();
        this.BLEch7button = GameObject.Find("BLEch7Button").GetComponentInChildren<Button>();


        this.canvas = GameObject.Find("Canvas");
        this.ble = GameObject.Find("BLE");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetInputLo_Valve()
    {
        //InputFieldからテキスト情報を取得する
        //Camvas　→　InputFiled:onEndEditへ
        string Lo_Valve = this.input_Lo_Valve_value.text;
        CheckInputValue(Lo_Valve, "Lo_Valve", Lo_Valve_Max, Lo_Valve_Min);
        
    }

    public void GetInputLo_Evaporator()
    {
        string Lo_Evaporator = this.input_Lo_Evaporator_value.text;
        CheckInputValue(Lo_Evaporator, "Lo_Evaporator", Lo_Evaporator_Max, Lo_Evaporator_Min);

    }

    public void GetInputLo_Compressor()
    {
        string Lo_Compressor = this.input_Lo_Compressor_value.text;
        CheckInputValue(Lo_Compressor, "Lo_Compressor", Lo_Compressor_Max, Lo_Compressor_Min);

    }

    public void GetInputHi_Valve()
    {
        string Hi_Valve = this.input_Hi_Valve_value.text;
        CheckInputValue(Hi_Valve, "Hi_Valve", Hi_Valve_Max, Hi_Valve_Min);

    }

    public void GetInputHi_Evaporator()
    {
        string Hi_Evaporator = this.input_Hi_Evaporator_value.text;
        CheckInputValue(Hi_Evaporator, "Hi_Evaporator", Hi_Evaporator_Max, Hi_Evaporator_Min);

    }

    public void GetInputHi_Compressor()
    {
        string Hi_Compressor = this.input_Hi_Compressor_value.text;
        CheckInputValue(Hi_Compressor, "Hi_Compressor", Hi_Compressor_Max, Hi_Compressor_Min);

    }

    public void GetInputTimeMinute()
    {
        string Time_Minute = this.input_Time_Minute_value.text;
        CheckInputValue(Time_Minute, "Time_Minute", Time_Minute_Max, Time_Minute_Min);

    }


    public void defultButtonClick()
    {
        
        this.canvas.GetComponent<PressureL03>().time = 0.0f; //経過時間を初期化
        this.canvas.GetComponent<PressureL03>().minute = 0;
        this.canvas.GetComponent<PressureL03>().hour = 0;
        this.canvas.GetComponent<PressureL03>().day = 0;

        this.canvas.GetComponent<PressureL03>().separateCount = 0;//カウンタを初期化
        this.canvas.GetComponent<PressureL03>().barCount = 10;//バーの数を初期化
        this.canvas.GetComponent<PressureL03>().setBarColor(10);//バーの数を変更

    }

    public void setButtonClick()
    {

        this.canvas.GetComponent<PressureL03>().Lo_Valve_Alarm = this.Lo_Valve_Value;
        this.canvas.GetComponent<PressureL03>().Lo_Evaporator_Alarm = this.Lo_Evaporator_Value;
        this.canvas.GetComponent<PressureL03>().Lo_Compressor_Alarm = this.Lo_Compressor_Value;
        this.canvas.GetComponent<PressureL03>().Hi_Valve_Alarm = this.Hi_Valve_Value;
        this.canvas.GetComponent<PressureL03>().Hi_Evaporator_Alarm = this.Hi_Evaporator_Value;
        this.canvas.GetComponent<PressureL03>().Hi_Compressor_Alarm = this.Hi_Compressor_Value;

        this.canvas.GetComponent<PressureL03>().time = 0.0f; //経過時間を初期化
        this.canvas.GetComponent<PressureL03>().minute = 0;
        this.canvas.GetComponent<PressureL03>().hour = 0;
        this.canvas.GetComponent<PressureL03>().day = 0;

        this.canvas.GetComponent<PressureL03>().separateCount = 0;//カウンタを初期化
        this.canvas.GetComponent<PressureL03>().barCount = 10;//バーの数を初期化
        this.canvas.GetComponent<PressureL03>().setBarColor(10);//バーの数を変更

        this.canvas.GetComponent<PressureL03>().separateTime = this.separateTime;



    }

    public void blech1ButtonClick()
    {
        bool scanButton = this.ble.GetComponent<BLEManager>()._scanch1button;

        if (!scanButton)
        {
            this.ble.GetComponent<BLEManager>().Start();
            this.ble.GetComponent<BLEManager>()._scanch1button = true;
        }
    }

    public void blech2ButtonClick()
    {
        bool scanButton = this.ble.GetComponent<BLEManagerB>()._scanch2button;

        if (!scanButton)
        {
            this.ble.GetComponent<BLEManagerB>().Start();
            this.ble.GetComponent<BLEManagerB>()._scanch2button = true;
        }
        
    }

    public void blech3ButtonClick()
    {
        bool scanButton = this.ble.GetComponent<BLEManagerC>()._scanch3button;

        if (!scanButton)
        {
            this.ble.GetComponent<BLEManagerC>().Start();
            this.ble.GetComponent<BLEManagerC>()._scanch3button = true;
        }

    }

    public void blech4ButtonClick()
    {
        bool scanButton = this.ble.GetComponent<BLEManagerD>()._scanch4button;

        if (!scanButton)
        {
            this.ble.GetComponent<BLEManagerD>().Start();
            this.ble.GetComponent<BLEManagerD>()._scanch4button = true;
        }

    }

    public void blech5ButtonClick()
    {
        bool scanButton = this.ble.GetComponent<BLEManagerE>()._scanch5button;

        if (!scanButton)
        {
            this.ble.GetComponent<BLEManagerE>().Start();
            this.ble.GetComponent<BLEManagerE>()._scanch5button = true;
        }

    }

    public void blech6ButtonClick()
    {
        bool scanButton = this.ble.GetComponent<BLEManagerF>()._scanch6button;

        if (!scanButton)
        {
            this.ble.GetComponent<BLEManagerF>().Start();
            this.ble.GetComponent<BLEManagerF>()._scanch6button = true;
        }

    }

    public void blech7ButtonClick()
    {
        bool scanButton = this.ble.GetComponent<BLEManagerG>()._scanch7button;

        if (!scanButton)
        {
            this.ble.GetComponent<BLEManagerG>().Start();
            this.ble.GetComponent<BLEManagerG>()._scanch7button = true;
        }

    }


    private void CheckInputValue(string input_value,string input_num ,float max,float min)
    {
        
        float input_value_float = float.Parse(input_value);


        switch (input_num)
        {
            case "Lo_Valve":
                if (input_value_float >= max)
                {
                    this.input_Lo_Valve_value.text = "2.0";
                    this.Lo_Valve_Value = 2.0f;
                }
                else if (input_value_float <= min)
                {
                    this.input_Lo_Valve_value.text = "0.0";
                    this.Lo_Valve_Value = 0.0f;
                }
                else
                {
                    this.input_Lo_Valve_value.text = input_value_float.ToString("0.0");
                    this.Lo_Valve_Value = float.Parse(this.input_Lo_Valve_value.text);
                }
               
                break;

            case "Lo_Evaporator":
                if (input_value_float >= max)
                {
                    this.input_Lo_Evaporator_value.text = "2.0";
                    this.Lo_Evaporator_Value = 2.0f;
                }
                else if (input_value_float <= min)
                {
                    this.input_Lo_Evaporator_value.text = "0.0";
                    this.Lo_Evaporator_Value = 0.0f;
                }
                else
                {
                    this.input_Lo_Evaporator_value.text = input_value_float.ToString("0.0");
                    this.Lo_Evaporator_Value = float.Parse(this.input_Lo_Evaporator_value.text);
                }
                
                break;

            case "Lo_Compressor":
                if (input_value_float >= max)
                {
                    this.input_Lo_Compressor_value.text = "4.0";
                    this.Lo_Compressor_Value = 4.0f;
                }
                else if (input_value_float <= min)
                {
                    this.input_Lo_Compressor_value.text = "0.0";
                    this.Lo_Compressor_Value = 0.0f;
                }
                else
                {
                    this.input_Lo_Compressor_value.text = input_value_float.ToString("0.0");
                    this.Lo_Compressor_Value = float.Parse(this.input_Lo_Compressor_value.text);
                }
               
                break;

            case "Hi_Valve":
                if (input_value_float >= max)
                {
                    this.input_Hi_Valve_value.text = "4.0";
                    this.Hi_Valve_Value = 4.0f;
                }
                else if (input_value_float <= min)
                {
                    this.input_Hi_Valve_value.text = "0.0";
                    this.Hi_Valve_Value = 0.0f;
                }
                else
                {
                    this.input_Hi_Valve_value.text = input_value_float.ToString("0.0");
                    this.Hi_Valve_Value = float.Parse(this.input_Hi_Valve_value.text);
                }
                
                break;

            case "Hi_Evaporator":
                if (input_value_float >= max)
                {
                    this.input_Hi_Evaporator_value.text = "4.0";
                    this.Hi_Evaporator_Value = 4.0f;
                }
                else if (input_value_float <= min)
                {
                    this.input_Hi_Evaporator_value.text = "0.0";
                    this.Hi_Evaporator_Value = 0.0f;
                }
                else
                {
                    this.input_Hi_Evaporator_value.text = input_value_float.ToString("0.0");
                    this.Hi_Evaporator_Value = float.Parse(this.input_Hi_Evaporator_value.text);
                }
                
                break;

            case "Hi_Compressor":
                if (input_value_float >= max)
                {
                    this.input_Hi_Compressor_value.text = "6.0";
                    this.Hi_Compressor_Value = 6.0f;
                }
                else if (input_value_float <= min)
                {
                    this.input_Hi_Compressor_value.text = "0.0";
                    this.Hi_Compressor_Value = 0.0f;
                }
                else
                {
                    this.input_Hi_Compressor_value.text = input_value_float.ToString("0.0");
                    this.Hi_Compressor_Value = float.Parse(this.input_Hi_Compressor_value.text);
                }
                
                break;

            case "Time_Minute":
                if (input_value_float >= max)
                {
                    this.input_Time_Minute_value.text = "10";
                    this.Time_Minute_Value = 10;
                }
                else if (input_value_float <= min)
                {
                    this.input_Time_Minute_value.text = "1";
                    this.Time_Minute_Value = 1;
                }
                else
                {
                    this.input_Time_Minute_value.text = input_value_float.ToString("00");
                    this.Time_Minute_Value = int.Parse(this.input_Time_Minute_value.text);
                }
                this.separateTime = (this.Time_Minute_Value * 3600) / 10;


                break;
        }

    }

}
