using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;  //시리얼통신을 위해 추가해줘야 함

namespace JGJ_Serial
{

    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        //쓰레드 충돌 방지로 두개의 쓰레드로 수신 데이터 처리하게 설정
        private void Th1(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(Th2));  
        }
        //int 형식을 string형식으로 변환하여 출력 시리얼 버터에 수신된 데이타를 Th1 읽기
        private void Th2(object s, EventArgs e)
        {
            int ReceiveData = serialPort1.ReadByte();
            richTextBox1.Text = richTextBox1.Text + string.Format("{0:X2}", ReceiveData);  
        }

        // 메인 폼 로드시 시리얼 com정보를 콤보박스에 추출
        private void Form1_Load(object sender, EventArgs e)
        {
            
            comboBox1.DataSource = SerialPort.GetPortNames();
        }
        // 연결하기 버튼 클릭시 연결정보 설정
        private void button1_Click(object sender, EventArgs e)
        {
            
            if (!serialPort1.IsOpen)
            {

                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = int.Parse(comboBox2.Text);
                serialPort1.DataBits = 8;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Parity = Parity.None;
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(Th1);

                serialPort1.Open();

                connect_info.ForeColor = Color.Black;
                connect_info.Text = "포트가 열렸습니다";
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
            }
            else
            {
                connect_info.ForeColor = Color.Red;
                connect_info.Text = "포트가 이미 활성화 되어 있습니다";
            }
        }

        private void connect_info_Click(object sender, EventArgs e)
        {

        }

        // 시리얼 포트 닫는 부분 각 콤보박스 컨트롤
        private void button2_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();

                connect_info.ForeColor = Color.Black;
                connect_info.Text = "포트를 닫았습니다";
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
            }
            else
            {
                connect_info.ForeColor = Color.Red;
                connect_info.Text = "포트가 이미 닫혀 있습니다";
            }
        }

        // 전송 데이터(textBox1.Text)를 serialPort1.Write 이용해서 시리얼포트 대상으로 전송
        private void button4_Click(object sender, EventArgs e)
        {
            serialPort1.Write(textBox1.Text);
        }
    }
}

