using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;


public class BLEManager : MonoBehaviour
{
    //DeviceName,ServiceUUID,Characteristicをperipheral側と合わせる
    public string DeviceName = "ESP32";
    public string ServiceUUID = "4fafc201-1fb5-459e-8fcc-c5c9c331914b";
    public string Characteristic = "beb5483e-36e1-4688-b7f5-ea07361b26a8";

    private float _timeout = 1f;
    private States _state = States.None;
    private string _deviceAddress;
    private bool _foundID = false;
    private bool _connected = false;

    private byte[] _dataBytes = null;

    public float TemperatureData;

    public bool _scanch1button = false;


    enum States
    {
        None, Scan, Connect, Subscribe, Unsubscribe, Disconnect
    }

    void Initialize()
    {
        this._state = States.None;
        this._deviceAddress = null;
        this._foundID = false;
        this._connected = false;
        this._dataBytes = null;
    }


    void SetState(States newState, float timeout)
    {
        this._state = newState;
        this._timeout = timeout; ;

    }

    //public Text textParameter;
    public Button BLEch1button;
    public Text BLEch1buttonText;

    // Start is called before the first frame update
    public void Start()
    {
        this.BLEch1button = GameObject.Find("BLEch1Button").GetComponentInChildren<Button>();
        this.BLEch1buttonText = BLEch1button.GetComponentInChildren<Text>();

        this.BLEch1button.image.color = Color.gray;
        this.BLEch1buttonText.text = "None";

        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {
            SetState(States.Scan, 0.1f);

        }, (error) => {
            BluetoothLEHardwareInterface.Log("Error: " + error);
        });

        //textParameter.text = "None_start";
        //textParameter.text += Environment.NewLine;
        Debug.Log("None_start");

    }

    // Update is called once per frame
    void Update()
    {
        if (this._timeout > 0f)
        {
            this._timeout -= Time.deltaTime;

            if (this._timeout <= 0f)
            {
                this._timeout = 0f;

                switch (_state)
                {
                    case States.None:

                        this.BLEch1button.image.color = Color.gray;
                        this.BLEch1buttonText.text = "None";

                        BluetoothLEHardwareInterface.Initialize(true, false, () =>
                        {
                            SetState(States.Scan, 0.1f);

                        }, (error) => {
                            BluetoothLEHardwareInterface.Log("Error: " + error);
                        });
                        break;

                    case States.Scan:

                        if (this._scanch1button)
                        {
                            //textParameter.text += "Scanning --> " + this.DeviceName;
                            //textParameter.text += Environment.NewLine;
                            Debug.Log("Scanning --> " + this.DeviceName);
                            this.BLEch1button.image.color = Color.yellow;
                            this.BLEch1buttonText.text = "Scannig...";

                            BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) =>
                            {
                                if (name.Contains(this.DeviceName))
                                {
                                    BluetoothLEHardwareInterface.StopScan();
                                    this._deviceAddress = address;
                                    SetState(States.Connect, 0.5f);

                                    //textParameter.text += "Found: " + name;
                                    //textParameter.text += Environment.NewLine;
                                    Debug.Log("Found: " + name);

                                    //textParameter.text += this._deviceAddress;
                                    //textParameter.text += Environment.NewLine;
                                    Debug.Log(this._deviceAddress);
                                }
                            }, null, false, false);

                            this._scanch1button = false;
                        }  

                        break;

                    case States.Connect:

                        this._foundID = false;

                        //textParameter.text += "Connect ...";
                        //textParameter.text += Environment.NewLine;
                        Debug.Log("Connect ...");
                        this.BLEch1button.image.color = Color.green;
                        this.BLEch1buttonText.text = "Connect...";

                        BluetoothLEHardwareInterface.ConnectToPeripheral(this._deviceAddress, null, null, (address, serviceUUID, characteristicUUID) =>
                        {
                            BluetoothLEHardwareInterface.Log("connectToPeripheral");

                            if (IsEqual(serviceUUID, this.ServiceUUID))
                            {
                                this._connected = true;
                                SetState(States.Subscribe, 2f);

                                //textParameter.text += "Conneted";
                                //textParameter.text += Environment.NewLine;
                                Debug.Log("Conneted");
                                
                            }
                        });
                        
                        break;


                    case States.Subscribe:

                        //textParameter.text += "Subscribe";
                        //textParameter.text += Environment.NewLine;
                        this.BLEch1buttonText.text = "Connected";

                        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(this._deviceAddress, this.ServiceUUID, this.Characteristic, null, (address, characteristicUUID, bytes) => {
                            this._state = States.None;
                            this._dataBytes = bytes;

                            if (this._dataBytes.Length == 4)
                            {
                                //string data = "";
                                //foreach (var b in this._dataBytes)
                                //data += b.ToString("X") + " ";

                                //textParameter.text += "Data: " + data;

                                int[] dataByte = new int[this._dataBytes.Length];

                                for (int i = 0; i < this._dataBytes.Length; i++)
                                {
                                    dataByte[i] = BitConverter.ToInt32(this._dataBytes, 0);
                                }

                                this.TemperatureData = (float)dataByte[0] / 10f;

                                //textParameter.text += "Data: " + datat[0];
                                //textParameter.text += Environment.NewLine;
                                this.BLEch1button.image.color = Color.cyan;
                                this.BLEch1buttonText.fontSize = 24;
                                this.BLEch1buttonText.text = "制御用温度\n温度計測中";
                            }
                            else
                            {
                                SetState(States.Unsubscribe, 4f);
                            }

                        });

                        break;

                    case States.Unsubscribe:
                        BluetoothLEHardwareInterface.UnSubscribeCharacteristic(this._deviceAddress, this.ServiceUUID, this.Characteristic, null);
                        SetState(States.Disconnect, 4f);
                        this.BLEch1button.image.color = Color.red;
                        this.BLEch1buttonText.text = "Undata";
                        break;

                    case States.Disconnect:
                        if (this._connected)
                        {
                            BluetoothLEHardwareInterface.DisconnectPeripheral(this._deviceAddress, (address) => {
                                BluetoothLEHardwareInterface.DeInitialize(() => {

                                    this._connected = false;
                                    this._state = States.None;
                                });
                            });
                        }
                        else
                        {
                            BluetoothLEHardwareInterface.DeInitialize(() => {

                                this._state = States.None;
                            });
                        }
                        this.BLEch1button.image.color = Color.red;
                        this.BLEch1buttonText.text = "Disconnect";
                        break;

                }

            }
        }
    }

    bool IsEqual(string uuid1, string uuid2)
    {
        return (uuid1.CompareTo(uuid2) == 0);
    }

}
