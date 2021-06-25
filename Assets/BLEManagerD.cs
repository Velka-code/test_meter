using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class BLEManagerD : MonoBehaviour
{
    //DeviceName,ServiceUUID,Characteristicをperipheral側と合わせる
    public string DeviceName = "ESP32D";
    public string ServiceUUID = "59ea376b-9e62-4c56-b856-13b1e06f4505";
    public string Characteristic = "513db915-d2d4-4836-b6e8-3234e402e052";

    private float _timeout = 1f;
    private States _state = States.None;
    private string _deviceAddress;
    private bool _foundID = false;
    private bool _connected = false;

    private byte[] _dataBytes = null;

    public float HighValveData;

    public bool _scanch4button = false;


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


    public Button BLEch4button;
    public Text BLEch4buttonText;

    // Start is called before the first frame update
    public void Start()
    {
        this.BLEch4button = GameObject.Find("BLEch4Button").GetComponentInChildren<Button>();
        this.BLEch4buttonText = BLEch4button.GetComponentInChildren<Text>();

        this.BLEch4button.image.color = Color.gray;
        this.BLEch4buttonText.text = "None";

        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {
            SetState(States.Scan, 0.1f);

        }, (error) => {
            BluetoothLEHardwareInterface.Log("Error: " + error);
        });

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

                        this.BLEch4button.image.color = Color.gray;
                        this.BLEch4buttonText.text = "None";

                        BluetoothLEHardwareInterface.Initialize(true, false, () =>
                        {
                            SetState(States.Scan, 0.1f);

                        }, (error) => {
                            BluetoothLEHardwareInterface.Log("Error: " + error);
                        });
                        break;

                    case States.Scan:


                        if (this._scanch4button)
                        {
                            this.BLEch4button.image.color = Color.yellow;
                            this.BLEch4buttonText.text = "Scannig...";

                            BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) =>
                            {
                                if (name.Contains(this.DeviceName))
                                {
                                    BluetoothLEHardwareInterface.StopScan();
                                    this._deviceAddress = address;
                                    SetState(States.Connect, 0.5f);

                                }
                            }, null, false, false);

                            this._scanch4button = false;
                        }

                        break;

                    case States.Connect:

                        this._foundID = false;


                        this.BLEch4button.image.color = Color.green;
                        this.BLEch4buttonText.text = "Connect...";

                        BluetoothLEHardwareInterface.ConnectToPeripheral(this._deviceAddress, null, null, (address, serviceUUID, characteristicUUID) =>
                        {
                            BluetoothLEHardwareInterface.Log("connectToPeripheral");

                            if (IsEqual(serviceUUID, this.ServiceUUID))
                            {
                                this._connected = true;
                                SetState(States.Subscribe, 2f);


                            }
                        });

                        break;


                    case States.Subscribe:

                        this.BLEch4buttonText.text = "Connected";

                        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(this._deviceAddress, this.ServiceUUID, this.Characteristic, null, (address, characteristicUUID, bytes) => {
                            this._state = States.None;
                            this._dataBytes = bytes;

                            if (this._dataBytes.Length == 4)
                            {

                                int[] dataByte = new int[this._dataBytes.Length];

                                for (int i = 0; i < this._dataBytes.Length; i++)
                                {
                                    dataByte[i] = BitConverter.ToInt32(this._dataBytes, 0);
                                }

                                this.HighValveData = (float)dataByte[0] / 10f;

                                this.BLEch4button.image.color = Color.cyan;
                                this.BLEch4buttonText.fontSize = 24;
                                this.BLEch4buttonText.text = "低温側膨張弁\n圧力計測中";
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
                        this.BLEch4button.image.color = Color.red;
                        this.BLEch4buttonText.text = "Undata";
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
                        this.BLEch4button.image.color = Color.red;
                        this.BLEch4buttonText.text = "Disconnect";
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

