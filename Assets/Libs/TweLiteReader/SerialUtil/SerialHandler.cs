using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

public class SerialHandler : MonoBehaviour
{
	// events
	public delegate void SerialDataHandler(int id);

	public static event SerialDataHandler Pressed 		   = (int id) => {};
	public static event SerialDataHandler Pressing 		   = (int id) => {};
	public static event SerialDataHandler Released 		   = (int id) => {};
	public static event SerialDataHandler SwitchOn 		   = (int id) => {};
	public static event SerialDataHandler SwitchOff 	   = (int id) => {};

	// serial
    public string portName = "/dev/tty.usbserial-AHY1U6SB";
	public int baudRate    = 115200;

	public int SwitchId = 12;
	private bool isSwitchOn = false;

	// test
	public bool isEmulation = false;
	public string emulationKey = "Fire1";
	public int emulationId = 0;

	private static SerialHandler instance = null;

	// serial data
	private SerialPort serialPort_;
	private bool isRunning_ = false;
	private Queue<string> messageQueue_ = new Queue<string>();
	private Thread thread_;
	private object guard_ = new object();

	private int PushDown = 2;
	private int PushUp = 1;

	private Dictionary<int, bool> isPressing_ = new Dictionary<int, bool>();

	public static SerialHandler Instance {
		get { return SerialHandler.instance; }
	}

	private Dictionary<int, int> lostCount_;

	void Awake()
	{
		if(instance == null) {
			instance = this;
		} else {
			Destroy( this );
		}
	}
	
	void Update()
	{
		Queue<string> tmpQueue = null;
		lock(guard_) {
			while(messageQueue_.Count > 0) {
				tmpQueue = new Queue<string>(messageQueue_);
				messageQueue_.Clear();
			}
		}

		if(tmpQueue	!= null) {
			while(tmpQueue.Count > 0) {
				string msg = tmpQueue.Dequeue();
				int id = System.Convert.ToInt16(msg.Substring(1, 2), 16);
				int value = System.Convert.ToInt16(msg.Substring(33, 2), 16);
				//int target_di = System.Convert.ToInt16(msg.Substring(35, 2), 16);
				//Debug.Log (id+ " : " + value);
				if(value == PushDown) {
					if(id == SwitchId) {
						if(isSwitchOn == false) {
							SwitchOn(id);
							isSwitchOn = true;
						}
					} else {
						isPressing_[id] = true;
						Pressed(id);
					}
				} else if(value == PushUp) {
					if(id == SwitchId) {
						if(isSwitchOn == true) {
							SwitchOff(id);
							isSwitchOn = false;
						}
					} else {
						isPressing_[id] = false;
						Released(id);
					}
				}
			}
		}

		foreach(var key in isPressing_.Keys) {
			if(isPressing_[key]) {
				Pressing(key);
			}
		}

		if(isEmulation) {
			if(Input.GetButtonDown(emulationKey)) {
				Pressed(emulationId);
			} else if(Input.GetButton(emulationKey)) {
				Pressing(emulationId);
			} else if(Input.GetButtonUp(emulationKey)) {
				Released(emulationId);
			}
		}
	}
	
	void OnDestroy()
	{
		Close_();
	}
	
	public static void Open()
	{
		// Debug.Log("try to Open");
		Instance.Open_();
	}

	public static void Close()
	{
		Instance.Close_();
	}

	private void Open_()
	{
		try {
			serialPort_ = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
			serialPort_.Open();
			
			isRunning_ = true;
			
			thread_ = new Thread(Read_);
			thread_.Start();
			
			Debug.Log("Connected!");
		} catch (System.Exception) {
			// Debug.LogError(e.Message);
		}
	}
	
	private void Close_()
	{
		isRunning_ = false;
		
		if (thread_ != null && thread_.IsAlive) {
			thread_.Join();
		}
		
		if (serialPort_ != null && serialPort_.IsOpen) {
			serialPort_.Close();
			serialPort_.Dispose();
		}
	}
	
	private void Read_()
	{
		while (isRunning_ && serialPort_ != null && serialPort_.IsOpen) {
			try {
				if (serialPort_.BytesToRead > 0) {
					lock(guard_) {
						messageQueue_.Enqueue(serialPort_.ReadLine());
					}
				}
				Thread.Sleep (0); // for avoiding busy loop
			} catch (System.Exception e) {
				 Debug.LogWarning(e.Message);
			}
		}
	}
}