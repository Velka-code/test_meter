using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class BLEManagerE : MonoBehaviour
{
    //DeviceName,ServiceUUID,Characteristicをperipheral側と合わせる
    public string DeviceName = "ESP32E";
    public string ServiceUUID = "b5621079-a030-44f8-a867-02cab34759c5";
    public string Characteristic = "3ff81721-acc8-43a8-85c6-cb7f93761839";

    private float _timeout = 1f;
    private States _state = States.None;
    private string _deviceAddress;
    private bool _foundID = false;
    private bool _connected = false;

    private byte[] _dataBytes = null;

    public float LowCompressorData;

    public bool _scanch5button = false;


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


    public Button BLEch5button;
    public Text BLEch5buttonText;

    // Start is called before the first frame update
    public void Start()
    {
        this.BLEch5button = GameObject.Find("BLEch5Button").GetComponentInChildren<Button>();
        this.BLEch5buttonText = BLEch5button.GetComponentInChildren<Text>();

        this.BLEch5button.image.color = Color.gray;
        this.BLEch5buttonText.text = "None";

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

                        this.BLEch5button.image.color = Color.gray;
                        this.BLEch5buttonText.text = "None";

                        BluetoothLEHardwareInterface.Initialize(true, false, () =>
                        {
                            SetState(States.Scan, 0.1f);

                        }, (error) => {
                            BluetoothLEHardwareInterface.Log("Error: " + error);
                        });
                        break;

                    case States.Scan:


                        if (this._scanch5button)
                        {
                            this.BLEch5button.image.color = Color.yellow;
                            this.BLEch5buttonText.text = "Scannig...";

                            BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) =>
                            {
                                if (name.Contains(this.DeviceName))
                                {
                                    BluetoothLEHardwareInterface.StopScan();
                                    this._deviceAddress = address;
                                    SetState(States.Connect, 0.5f);

                                }
                            }, null, false, false);

                            this._scanch5button = false;
                        }

                        break;

                    case States.Connect:

                        this._foundID = false;


                        this.BLEch5button.image.color = Color.green;
                        this.BLEch5buttonText.text = "Connect...";

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

                        this.BLEch5buttonText.text = "Connected";

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

                                this.LowCompressorData = (float)dataByte[0] / 10f;

                                this.BLEch5button.image.color = Color.cyan;
                                this.BLEch5buttonText.fontSize = 24;
                                this.BLEch5buttonText.text = "高温側圧縮機\n圧力計測中";
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
                        this.BLEch5button.image.color = Color.red;
                        this.BLEch5buttonText.text = "Undata";
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
                        this.BLEch5button.image.color = Color.red;
                        this.BLEch5buttonText.text = "Disconnect";
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
