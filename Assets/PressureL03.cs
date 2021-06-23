using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressureL03 : MonoBehaviour
{
    private float defultPos = 45.0f;

    private float min_HiAngle = 4.5f * -10.0f;
    private float min_MidAngle = 6.75f * -10.0f;
    private float min_LoAngle = 13.5f * -10.0f;

    private float Now_LoValvePos = 0.0f;
    private float Now_HiValvePos = 0.0f;
    private float Now_LoEvaporatorPos = 0.0f;
    private float Now_HiEvaporatorPos = 0.0f;
    private float Now_LoCompressorPos = 0.0f;
    private float Now_HiCompressorPos = 0.0f;


    private Transform transform_Lo_Valve_pointer;
    private Transform transform_Lo_Evaporator_pointer;
    private Transform transform_Lo_Compressor_pointer;
    private Transform transform_Hi_Valve_pointer;
    private Transform transform_Hi_Evaporator_pointer;
    private Transform transform_Hi_Compressor_pointer;

    private float dataPos;
    private float Angle;

    private string[] barName = { "bar1", "bar2", "bar3", "bar4", "bar5", "bar6", "bar7", "bar8", "bar9", "bar10" };
    private Image[] bar = new Image[10];
    
    public int barCount = 10;//panel Buttonからも変更

    private string[] segD1 = { "segParent_day1/7seg_a", "segParent_day1/7seg_b", "segParent_day1/7seg_c", "segParent_day1/7seg_d", "segParent_day1/7seg_e", "segParent_day1/7seg_f", "segParent_day1/7seg_g"};
    private string[] segD2 = { "segParent_day2/7seg_a", "segParent_day2/7seg_b", "segParent_day2/7seg_c", "segParent_day2/7seg_d", "segParent_day2/7seg_e", "segParent_day2/7seg_f", "segParent_day2/7seg_g" };
    private string[] segH1 = { "segParent_hour1/7seg_a", "segParent_hour1/7seg_b", "segParent_hour1/7seg_c", "segParent_hour1/7seg_d", "segParent_hour1/7seg_e", "segParent_hour1/7seg_f", "segParent_hour1/7seg_g" };
    private string[] segH2 = { "segParent_hour2/7seg_a", "segParent_hour2/7seg_b", "segParent_hour2/7seg_c", "segParent_hour2/7seg_d", "segParent_hour2/7seg_e", "segParent_hour2/7seg_f", "segParent_hour2/7seg_g" };
    private string[] segM1 = { "segParent_min1/7seg_a", "segParent_min1/7seg_b", "segParent_min1/7seg_c", "segParent_min1/7seg_d", "segParent_min1/7seg_e", "segParent_min1/7seg_f", "segParent_min1/7seg_g" };
    private string[] segM2 = { "segParent_min2/7seg_a", "segParent_min2/7seg_b", "segParent_min2/7seg_c", "segParent_min2/7seg_d", "segParent_min2/7seg_e", "segParent_min2/7seg_f", "segParent_min2/7seg_g" };

    private Image[] segDay1 = new Image[7];
    private Image[] segDay2 = new Image[7];
    private Image[] segHour1 = new Image[7];
    private Image[] segHour2 = new Image[7];
    private Image[] segMin1 = new Image[7];
    private Image[] segMin2 = new Image[7];


    private string[] seg1 = { "segParent1/7seg_a", "segParent1/7seg_b", "segParent1/7seg_c", "segParent1/7seg_d", "segParent1/7seg_e", "segParent1/7seg_f", "segParent1/7seg_g", "segParent1/7seg_dp" };
    private string[] seg2 = { "segParent2/7seg_a", "segParent2/7seg_b", "segParent2/7seg_c", "segParent2/7seg_d", "segParent2/7seg_e", "segParent2/7seg_f", "segParent2/7seg_g", "segParent2/7seg_dp" };
    private string[] seg3 = { "segParent3/7seg_a", "segParent3/7seg_b", "segParent3/7seg_c", "segParent3/7seg_d", "segParent3/7seg_e", "segParent3/7seg_f", "segParent3/7seg_g", "segParent3/7seg_dp" };
    private string[] seg4 = { "segParent4/7seg_a", "segParent4/7seg_b", "segParent4/7seg_c", "segParent4/7seg_d", "segParent4/7seg_e", "segParent4/7seg_f", "segParent4/7seg_g", "segParent4/7seg_dp" };

    private Image[] segH = new Image[8];
    private Image[] segT = new Image[8];
    private Image[] segO = new Image[8];
    private Image[] segD = new Image[8];

    public float time = 0f;//panel Buttonからも変更

    private int oldsecond = 0;

    //上限を手動で設定（チェック用）
    private int dayLimit = 0;
    private int hourLimit = 0;
    private int minuteLimit =1;
    private int secondLimit = 0;

    private int totalSecond;

    private int second;

    public int minute;
    public int hour;
    public int day;


    public int separateTime;

    private int separate; 
    private bool separateCountFlag = true;

    public int separateCount = 0;//panel Buttonからも変更

    private Text statusText;

    private CanvasGroup panel;
    private bool panelDisplay = false;

    GameObject Data;
    dataGeneretor datascript;

    private float longTaptime = 1.0f;
    private float nowTaptime = 0.0f;


    public float Lo_Valve_Alarm;
    public float Lo_Evaporator_Alarm;
    public float Lo_Compressor_Alarm;
    public float Hi_Valve_Alarm;    
    public float Hi_Evaporator_Alarm;    
    public float Hi_Compressor_Alarm;
    

    //int number = 1;


    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            Debug.Log("WindowsEditor");
        }else if(Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("android");
        }


        Data = GameObject.Find("Data");
        datascript = Data.GetComponent<dataGeneretor>();

        this.panel = GameObject.Find("Panel").GetComponentInChildren<CanvasGroup>();

        //設定画面を消す
        this.panel.alpha = 0.0f;

        this.statusText = GameObject.Find("status_Label").GetComponentInChildren<Text>();

        this.transform_Lo_Valve_pointer = GameObject.Find("Lo_Valve_pointer").GetComponentInChildren<RectTransform>();
        this.transform_Lo_Evaporator_pointer = GameObject.Find("Lo_Evaporator_pointer").GetComponentInChildren<RectTransform>();
        this.transform_Lo_Compressor_pointer = GameObject.Find("Lo_Compressor_pointer").GetComponentInChildren<RectTransform>();

        this.transform_Hi_Valve_pointer = GameObject.Find("Hi_Valve_pointer").GetComponentInChildren<RectTransform>();
        this.transform_Hi_Evaporator_pointer = GameObject.Find("Hi_Evaporator_pointer").GetComponentInChildren<RectTransform>();
        this.transform_Hi_Compressor_pointer = GameObject.Find("Hi_Compressor_pointer").GetComponentInChildren<RectTransform>();

        this.transform_Lo_Valve_pointer.Rotate(new Vector3(0, 0, defultPos));
        this.transform_Lo_Evaporator_pointer.Rotate(new Vector3(0, 0, defultPos));
        this.transform_Lo_Compressor_pointer.Rotate(new Vector3(0, 0, defultPos));

        this.transform_Hi_Valve_pointer.Rotate(new Vector3(0, 0, defultPos));
        this.transform_Hi_Evaporator_pointer.Rotate(new Vector3(0, 0, defultPos));
        this.transform_Hi_Compressor_pointer.Rotate(new Vector3(0, 0, defultPos));



        for(int barArryCount=0; barArryCount < bar.Length; barArryCount++)
        {
            this.bar[barArryCount] = GameObject.Find(this.barName[barArryCount]).GetComponentInChildren<Image>();

        }

        //バーの初期化
        setBarColor(10);
        //連続動作時間の上限を秒で算出し、バーの数と同じように10分割している。
        this.separateTime = ((this.dayLimit * 24 * 3600) + (this.hourLimit * 3600) + (this.minuteLimit * 60) + this.secondLimit) / 10;


        for (int i = 0; i < segD1.Length; i++)
        {
            this.segDay1[i] = GameObject.Find(this.segD1[i]).GetComponentInChildren<Image>();

        }
        for (int i = 0; i < segD2.Length; i++)
        {
            this.segDay2[i] = GameObject.Find(this.segD2[i]).GetComponentInChildren<Image>();

        }
        for (int i = 0; i < segH1.Length; i++)
        {
            this.segHour1[i] = GameObject.Find(this.segH1[i]).GetComponentInChildren<Image>();

        }
        for (int i = 0; i < segH2.Length; i++)
        {
            this.segHour2[i] = GameObject.Find(this.segH2[i]).GetComponentInChildren<Image>();

        }
        for (int i = 0; i < segM1.Length; i++)
        {
            this.segMin1[i] = GameObject.Find(this.segM1[i]).GetComponentInChildren<Image>();

        }
        for (int i = 0; i < segM2.Length; i++)
        {
            this.segMin2[i] = GameObject.Find(this.segM2[i]).GetComponentInChildren<Image>();

        }

        //連続動作時間の初期化
        timeSeg(0, 0, 0);


        for (int i = 0; i < seg1.Length; i++)
        {
            this.segH[i] = GameObject.Find(this.seg1[i]).GetComponentInChildren<Image>();

        }
        for (int i = 0; i < seg2.Length; i++)
        {
            this.segT[i] = GameObject.Find(this.seg2[i]).GetComponentInChildren<Image>();

        }
        for (int i = 0; i < seg3.Length; i++)
        {
            this.segO[i] = GameObject.Find(this.seg3[i]).GetComponentInChildren<Image>();

        }
        for (int i = 0; i < seg4.Length; i++)
        {
            this.segD[i] = GameObject.Find(this.seg4[i]).GetComponentInChildren<Image>();

        }

        //アラーム判定　初期値設定
        this.Lo_Valve_Alarm = 2.0f;
        this.Lo_Evaporator_Alarm = 2.0f;
        this.Lo_Compressor_Alarm = 4.0f;
        this.Hi_Valve_Alarm = 4.0f;
        this.Hi_Evaporator_Alarm = 4.0f;
        this.Hi_Compressor_Alarm = 6.0f;

        //ステータス情報の表示
        setAlarm("動作中");

    }

    // Update is called once per frame
    void Update()
    {
        if (datascript.dummy)
        {
            datascript.Pointer_value();

            this.dataPos = datascript.LowValve;
            this.Angle = (this.dataPos - this.Now_LoValvePos) * this.min_LoAngle;
            this.transform_Lo_Valve_pointer.Rotate(new Vector3(0, 0, this.Angle));
            this.Now_LoValvePos = this.dataPos;

            this.dataPos = datascript.LowEvaporator;
            this.Angle = (this.dataPos - this.Now_LoEvaporatorPos) * this.min_LoAngle;
            this.transform_Lo_Evaporator_pointer.Rotate(new Vector3(0, 0, this.Angle));
            this.Now_LoEvaporatorPos = this.dataPos;

            this.dataPos = datascript.LowCompressor;
            this.Angle = (this.dataPos - this.Now_LoCompressorPos) * this.min_MidAngle;
            this.transform_Lo_Compressor_pointer.Rotate(new Vector3(0, 0, this.Angle));
            this.Now_LoCompressorPos = this.dataPos;

            this.dataPos = datascript.HighValve;
            this.Angle = (this.dataPos - this.Now_HiValvePos) * this.min_MidAngle;
            this.transform_Hi_Valve_pointer.Rotate(new Vector3(0, 0, this.Angle));
            this.Now_HiValvePos = this.dataPos;

            this.dataPos = datascript.HighEvaporator;
            this.Angle = (this.dataPos - this.Now_HiEvaporatorPos) * this.min_MidAngle;
            this.transform_Hi_Evaporator_pointer.Rotate(new Vector3(0, 0, this.Angle));
            this.Now_HiEvaporatorPos = this.dataPos;

            this.dataPos = datascript.HighCompressor;
            this.Angle = (this.dataPos - this.Now_HiCompressorPos) * this.min_HiAngle;
            this.transform_Hi_Compressor_pointer.Rotate(new Vector3(0, 0, this.Angle));
            this.Now_HiCompressorPos = this.dataPos;



            /*温度データの取得
             * 温度データは、float型のため10倍してから整数にキャストしている。
             * このとき小数点以下は削除される。
             */
            float number = datascript.Temperature;
            number = number * 10;

            int num = (int)number;

            Segment(num);
        }


        /*
         * 連続動作時間のカウント開始
         */

        this.time += Time.deltaTime;

        this.second = (int)this.time;


        if(this.oldsecond != this.second)
        {
            if (this.second >= 60)
            {
                this.minute++;
                this.time = 0f;

                if(this.minute >= 60)
                {
                    this.hour++;
                    this.minute = 0;

                    if(this.hour >= 24)
                    {
                        this.day++;
                        this.hour = 0;

                        if(this.day >= 99)
                        {
                            this.day = 0;
                        }
                    }
                }

            }

            timeSeg(this.hour, this.minute, this.second);
            //Debug.Log(this.hour + "時" + this.minute + "分" + this.second + "秒");

            //timeSeg(this.day, this.hour, this.minute);
            //Debug.Log(this.day +"日"+ this.hour +"時"+ this.minute + "分" + this.second + "秒");

            //初期化ボタンが押されたときtotalSecond=0になる
            this.totalSecond = this.day * 24 * 3600 + this.hour * 3600 + this.minute * 60 + (int)this.time;


            //初期化ボタンが押されたときseparateCount=0になる
            if (this.separateCount >= 10)
            {
                this.separateCountFlag = false;
                setAlarm("連続運転注意");

            }
            else
            {
                this.separateCountFlag = true;
                setAlarm("動作中");
            }

            //設定ボタンが押されるとseparateTimeを変更する。
            this.separate = this.totalSecond - this.separateTime * this.separateCount;
            //Debug.Log("totalSecond: " + totalSecond + "separateTime: " + separateTime +"separateCount: " + separateCount);

            if (this.separate >= this.separateTime && this.separateCountFlag == true)
            {
                this.barCount = this.barCount - 1;

                setBarColor(this.barCount);
                this.separateCount++;

            }

            this.oldsecond = this.second;
        }

        //マウスのクリック検出
        if (Input.GetMouseButton(0))
        {
            this.nowTaptime += Time.deltaTime;

            if(this.nowTaptime >= this.longTaptime && !this.panelDisplay)
            {
                this.nowTaptime = 0.0f;
                this.panel.alpha = 0.9f;
                this.panelDisplay = true;
            }
            else if (this.nowTaptime >= this.longTaptime && this.panelDisplay)
            {
                this.nowTaptime = 0.0f;
                this.panel.alpha = 0.0f;
                this.panelDisplay = false;
            }

        }else if (Input.GetMouseButtonUp(0))
        {
            this.nowTaptime = 0.0f;
        }

    }

    public void setAlarm(string num)
    {

        string alarmoffString = "#85c31f";//緑色
        Color alarmoffColor;
        ColorUtility.TryParseHtmlString(alarmoffString, out alarmoffColor);

        string alarmonString = "#FF0000";//赤色
        Color alarmonColor;
        ColorUtility.TryParseHtmlString(alarmonString, out alarmonColor);

        switch (num)
        {
            case "動作中":
                this.statusText.color = alarmoffColor;
                this.statusText.text = "動作中";
                break;
            case "連続運転注意":
                this.statusText.color = alarmonColor;
                this.statusText.text = "連続運転注意";
                break;
        }

    }

    /*
     * 連続動作時間（残り時間）をバーグラフ表示する
     * imageは白色画像のため初期化に灰色にしている。その後、赤色で残量を表示
     * 引数は、赤色の残量分で10段階としている。
     */
    public void setBarColor(int num)
    {
        string baroffString = "#595757";//灰色
        Color defultcolor;
        ColorUtility.TryParseHtmlString(baroffString, out defultcolor);

        for (int i = 0; i < bar.Length; i++)
        {
            bar[i].color = defultcolor;
        }

        string baronString = "#FF0000";//赤色
        Color baronColor;
        ColorUtility.TryParseHtmlString(baronString, out baronColor);


        for (int i = 0; i < num; i++)
        {
            bar[i].color = baronColor;
        }
    }

    public void timeSeg(int day, int hour, int min)
    {
        int firstDayNum;
        int lastDayNum;
        int firstHourNum;
        int lastHourNum;
        int firstMinNum;
        int lastMinNum;

        string segoffString = "#595757";//灰色
        Color offColor;
        ColorUtility.TryParseHtmlString(segoffString, out offColor);

        for (int i = 0; i < segDay1.Length; i++)
        {
            segDay1[i].color = offColor;
        }

        for (int i = 0; i < segDay2.Length; i++)
        {
            segDay2[i].color = offColor;
        }

        for (int i = 0; i < segHour1.Length; i++)
        {
            segHour1[i].color = offColor;
        }

        for (int i = 0; i < segHour2.Length; i++)
        {
            segHour2[i].color = offColor;
        }

        for (int i = 0; i < segMin1.Length; i++)
        {
            segMin1[i].color = offColor;
        }

        for (int i = 0; i < segMin2.Length; i++)
        {
            segMin2[i].color = offColor;
        }

        string segonString = "#FF0000";//赤色
        Color onColor;
        ColorUtility.TryParseHtmlString(segonString, out onColor);

        firstDayNum = day / 10;
        lastDayNum = day - (firstDayNum * 10);

        firstHourNum = hour / 10;
        lastHourNum = hour - (firstHourNum * 10);

        firstMinNum = min / 10;
        lastMinNum = min - (firstMinNum * 10);


        switch (firstDayNum)
        {
            case 0:
                segDay1[0].color = segDay1[1].color = segDay1[2].color = segDay1[3].color = segDay1[4].color = segDay1[5].color = onColor;
                break;

            case 1:
                segDay1[1].color = segDay1[2].color = onColor;
                break;
            case 2:
                segDay1[0].color = segDay1[1].color = segDay1[6].color = segDay1[4].color = segDay1[3].color = onColor;
                break;
            case 3:
                segDay1[0].color = segDay1[1].color = segDay1[6].color = segDay1[2].color = segDay1[3].color = onColor;
                break;
            case 4:
                segDay1[5].color = segDay1[1].color = segDay1[2].color = segDay1[6].color = onColor;
                break;
            case 5:
                segDay1[0].color = segDay1[5].color = segDay1[6].color = segDay1[2].color = segDay1[3].color = onColor;
                break;
            case 6:
                segDay1[0].color = segDay1[5].color = segDay1[4].color = segDay1[6].color = segDay1[2].color = segDay1[3].color = onColor;
                break;
            case 7:
                segDay1[0].color = segDay1[1].color = segDay1[2].color = onColor;
                break;
            case 8:
                segDay1[0].color = segDay1[1].color = segDay1[2].color = segDay1[3].color = segDay1[4].color = segDay1[5].color = segDay1[6].color = onColor;
                break;
            case 9:
                segDay1[0].color = segDay1[1].color = segDay1[2].color = segDay1[3].color = segDay1[5].color = segDay1[6].color = onColor;
                break;
        }

        switch (lastDayNum)
        {
            case 0:
                segDay2[0].color = segDay2[1].color = segDay2[2].color = segDay2[3].color = segDay2[4].color = segDay2[5].color = onColor;
                break;

            case 1:
                segDay2[1].color = segDay2[2].color = onColor;
                break;
            case 2:
                segDay2[0].color = segDay2[1].color = segDay2[6].color = segDay2[4].color = segDay2[3].color = onColor;
                break;
            case 3:
                segDay2[0].color = segDay2[1].color = segDay2[6].color = segDay2[2].color = segDay2[3].color = onColor;
                break;
            case 4:
                segDay2[5].color = segDay2[1].color = segDay2[2].color = segDay2[6].color = onColor;
                break;
            case 5:
                segDay2[0].color = segDay2[5].color = segDay2[6].color = segDay2[2].color = segDay2[3].color = onColor;
                break;
            case 6:
                segDay2[0].color = segDay2[5].color = segDay2[4].color = segDay2[6].color = segDay2[2].color = segDay2[3].color = onColor;
                break;
            case 7:
                segDay2[0].color = segDay2[1].color = segDay2[2].color = onColor;
                break;
            case 8:
                segDay2[0].color = segDay2[1].color = segDay2[2].color = segDay2[3].color = segDay2[4].color = segDay2[5].color = segDay2[6].color = onColor;
                break;
            case 9:
                segDay2[0].color = segDay2[1].color = segDay2[2].color = segDay2[3].color = segDay2[5].color = segDay2[6].color = onColor;
                break;
        }
        switch (firstHourNum)
        {
            case 0:
                segHour1[0].color = segHour1[1].color = segHour1[2].color = segHour1[3].color = segHour1[4].color = segHour1[5].color = onColor;
                break;

            case 1:
                segHour1[1].color = segHour1[2].color = onColor;
                break;
            case 2:
                segHour1[0].color = segHour1[1].color = segHour1[6].color = segHour1[4].color = segHour1[3].color = onColor;
                break;
            case 3:
                segHour1[0].color = segHour1[1].color = segHour1[6].color = segHour1[2].color = segHour1[3].color = onColor;
                break;
            case 4:
                segHour1[5].color = segHour1[1].color = segHour1[2].color = segHour1[6].color = onColor;
                break;
            case 5:
                segHour1[0].color = segHour1[5].color = segHour1[6].color = segHour1[2].color = segHour1[3].color = onColor;
                break;
            case 6:
                segHour1[0].color = segHour1[5].color = segHour1[4].color = segHour1[6].color = segHour1[2].color = segHour1[3].color = onColor;
                break;
            case 7:
                segHour1[0].color = segHour1[1].color = segHour1[2].color = onColor;
                break;
            case 8:
                segHour1[0].color = segHour1[1].color = segHour1[2].color = segHour1[3].color = segHour1[4].color = segHour1[5].color = segHour1[6].color = onColor;
                break;
            case 9:
                segHour1[0].color = segHour1[1].color = segHour1[2].color = segHour1[3].color = segHour1[5].color = segHour1[6].color = onColor;
                break;
        }

        switch (lastHourNum)
        {
            case 0:
                segHour2[0].color = segHour2[1].color = segHour2[2].color = segHour2[3].color = segHour2[4].color = segHour2[5].color = onColor;
                break;

            case 1:
                segHour2[1].color = segHour2[2].color = onColor;
                break;
            case 2:
                segHour2[0].color = segHour2[1].color = segHour2[6].color = segHour2[4].color = segHour2[3].color = onColor;
                break;
            case 3:
                segHour2[0].color = segHour2[1].color = segHour2[6].color = segHour2[2].color = segHour2[3].color = onColor;
                break;
            case 4:
                segHour2[5].color = segHour2[1].color = segHour2[2].color = segHour2[6].color = onColor;
                break;
            case 5:
                segHour2[0].color = segHour2[5].color = segHour2[6].color = segHour2[2].color = segHour2[3].color = onColor;
                break;
            case 6:
                segHour2[0].color = segHour2[5].color = segHour2[4].color = segHour2[6].color = segHour2[2].color = segHour2[3].color = onColor;
                break;
            case 7:
                segHour2[0].color = segHour2[1].color = segHour2[2].color = onColor;
                break;
            case 8:
                segHour2[0].color = segHour2[1].color = segHour2[2].color = segHour2[3].color = segHour2[4].color = segHour2[5].color = segHour2[6].color = onColor;
                break;
            case 9:
                segHour2[0].color = segHour2[1].color = segHour2[2].color = segHour2[3].color = segHour2[5].color = segHour2[6].color = onColor;
                break;
        }
        switch (firstMinNum)
        {
            case 0:
                segMin1[0].color = segMin1[1].color = segMin1[2].color = segMin1[3].color = segMin1[4].color = segMin1[5].color = onColor;
                break;

            case 1:
                segMin1[1].color = segMin1[2].color = onColor;
                break;
            case 2:
                segMin1[0].color = segMin1[1].color = segMin1[6].color = segMin1[4].color = segMin1[3].color = onColor;
                break;
            case 3:
                segMin1[0].color = segMin1[1].color = segMin1[6].color = segMin1[2].color = segMin1[3].color = onColor;
                break;
            case 4:
                segMin1[5].color = segMin1[1].color = segMin1[2].color = segMin1[6].color = onColor;
                break;
            case 5:
                segMin1[0].color = segMin1[5].color = segMin1[6].color = segMin1[2].color = segMin1[3].color = onColor;
                break;
            case 6:
                segMin1[0].color = segMin1[5].color = segMin1[4].color = segMin1[6].color = segMin1[2].color = segMin1[3].color = onColor;
                break;
            case 7:
                segMin1[0].color = segMin1[1].color = segMin1[2].color = onColor;
                break;
            case 8:
                segMin1[0].color = segMin1[1].color = segMin1[2].color = segMin1[3].color = segMin1[4].color = segMin1[5].color = segMin1[6].color = onColor;
                break;
            case 9:
                segMin1[0].color = segMin1[1].color = segMin1[2].color = segMin1[3].color = segMin1[5].color = segMin1[6].color = onColor;
                break;
        }

        switch (lastMinNum)
        {
            case 0:
                segMin2[0].color = segMin2[1].color = segMin2[2].color = segMin2[3].color = segMin2[4].color = segMin2[5].color = onColor;
                break;

            case 1:
                segMin2[1].color = segMin2[2].color = onColor;
                break;
            case 2:
                segMin2[0].color = segMin2[1].color = segMin2[6].color = segMin2[4].color = segMin2[3].color = onColor;
                break;
            case 3:
                segMin2[0].color = segMin2[1].color = segMin2[6].color = segMin2[2].color = segMin2[3].color = onColor;
                break;
            case 4:
                segMin2[5].color = segMin2[1].color = segMin2[2].color = segMin2[6].color = onColor;
                break;
            case 5:
                segMin2[0].color = segMin2[5].color = segMin2[6].color = segMin2[2].color = segMin2[3].color = onColor;
                break;
            case 6:
                segMin2[0].color = segMin2[5].color = segMin2[4].color = segMin2[6].color = segMin2[2].color = segMin2[3].color = onColor;
                break;
            case 7:
                segMin2[0].color = segMin2[1].color = segMin2[2].color = onColor;
                break;
            case 8:
                segMin2[0].color = segMin2[1].color = segMin2[2].color = segMin2[3].color = segMin2[4].color = segMin2[5].color = segMin2[6].color = onColor;
                break;
            case 9:
                segMin2[0].color = segMin2[1].color = segMin2[2].color = segMin2[3].color = segMin2[5].color = segMin2[6].color = onColor;
                break;
        }
    }



    private void Segment(int temp)
    {
        int firstNum;
        int middleNum;
        int lastNum;


        string segoffString = "#595757";//灰色
        Color offColor;
        ColorUtility.TryParseHtmlString(segoffString, out offColor);

        for(int i= 0; i < segH.Length; i++)
        {
            segH[i].color = offColor;
        }

        for (int i = 0; i < segT.Length; i++)
        {
            segT[i].color = offColor;
        }

        for (int i = 0; i < segO.Length; i++)
        {
            segO[i].color = offColor;
        }
        
        for (int i = 0; i < segD.Length; i++)
        {
            segD[i].color = offColor;
        }


        string segonString = "#85c31f";//緑色
        Color onColor;
        ColorUtility.TryParseHtmlString(segonString, out onColor);


        if( temp < 0)
        {
            segH[6].color = onColor;
            temp = temp * -1;
        }

        firstNum = temp / 100;
        middleNum = (temp - firstNum * 100) / 10;
        lastNum = temp - (firstNum * 100 + middleNum * 10);

        //Debug.Log(temp + " : " + firstNum + " : " + middleNum + " : " + lastNum);

        switch (firstNum)
        {
            case 0:
                segT[0].color = segT[1].color = segT[2].color = segT[3].color = segT[4].color = segT[5].color = onColor;
                break;

            case 1:
                segT[1].color = segT[2].color = onColor;
                break;
            case 2:
                segT[0].color = segT[1].color = segT[6].color = segT[4].color = segT[3].color = onColor;
                break;
            case 3:
                segT[0].color = segT[1].color = segT[6].color = segT[2].color = segT[3].color = onColor;
                break;
            case 4:
                segT[5].color = segT[1].color = segT[2].color = segT[6].color = onColor;
                break;
            case 5:
                segT[0].color = segT[5].color = segT[6].color = segT[2].color = segT[3].color = onColor;
                break;
            case 6:
                segT[0].color = segT[5].color = segT[4].color = segT[6].color = segT[2].color = segT[3].color = onColor;
                break;
            case 7:
                segT[0].color = segT[1].color = segT[2].color = onColor;
                break;
            case 8:
                segT[0].color = segT[1].color = segT[2].color = segT[3].color = segT[4].color = segT[5].color = segT[6].color = onColor;
                break;
            case 9:
                segT[0].color = segT[1].color = segT[2].color = segT[3].color = segT[5].color = segT[6].color = onColor;
                break;
        }



        switch (middleNum)
        {
            case 0:
                segO[0].color = segO[1].color = segO[2].color = segO[3].color = segO[4].color = segO[5].color = segO[7].color = onColor;
                break;

            case 1:
                segO[1].color = segO[2].color = segO[7].color = onColor;
                break;
            case 2:
                segO[0].color = segO[1].color = segO[6].color = segO[4].color = segO[3].color = segO[7].color = onColor;
                break;
            case 3:
                segO[0].color = segO[1].color = segO[6].color = segO[2].color = segO[3].color = segO[7].color = onColor;
                break;
            case 4:
                segO[5].color = segO[1].color = segO[2].color = segO[6].color = segO[7].color = onColor;
                break;
            case 5:
                segO[0].color = segO[5].color = segO[6].color = segO[2].color = segO[3].color = segO[7].color = onColor;
                break;
            case 6:
                segO[0].color = segO[5].color = segO[4].color = segO[6].color = segO[2].color = segO[3].color = segO[7].color = onColor;
                break;
            case 7:
                segO[0].color = segO[1].color = segO[2].color = segO[7].color = onColor;
                break;
            case 8:
                segO[0].color = segO[1].color = segO[2].color = segO[3].color = segO[4].color = segO[5].color = segO[6].color = segO[7].color = onColor;
                break;
            case 9:
                segO[0].color = segO[1].color = segO[2].color = segO[3].color = segO[5].color = segO[6].color = segO[7].color = onColor;
                break;
        }

        switch (lastNum)
        {
            case 0:
                segD[0].color = segD[1].color = segD[2].color = segD[3].color = segD[4].color = segD[5].color = onColor;
                break;

            case 1:
                segD[1].color = segD[2].color = onColor;
                break;
            case 2:
                segD[0].color = segD[1].color = segD[6].color = segD[4].color = segD[3].color = onColor;
                break;
            case 3:
                segD[0].color = segD[1].color = segD[6].color = segD[2].color = segD[3].color = onColor;
                break;
            case 4:
                segD[5].color = segD[1].color = segD[2].color = segD[6].color = onColor;
                break;
            case 5:
                segD[0].color = segD[5].color = segD[6].color = segD[2].color = segD[3].color = onColor;
                break;
            case 6:
                segD[0].color = segD[5].color = segD[4].color = segD[6].color = segD[2].color = segD[3].color = onColor;
                break;
            case 7:
                segD[0].color = segD[1].color = segD[2].color = onColor;
                break;
            case 8:
                segD[0].color = segD[1].color = segD[2].color = segD[3].color = segD[4].color = segD[5].color = segD[6].color = onColor;
                break;
            case 9:
                segD[0].color = segD[1].color = segD[2].color = segD[3].color = segD[5].color = segD[6].color = onColor;
                break;
        }

    }
}
