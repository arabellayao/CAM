using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
namespace WindowsFormsApp1
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            {
                var gen = new Genesis();

                string dbname = "genesis";
                string jobname = textBox1.Text;



                Console.WriteLine("Instantiated the Genesis Object...");



                string msg = "Welcome to the Java test Script...apparently its working...";
                Console.WriteLine(msg);

                gen.PAUSE(msg);

                gen.COM("create_entity,job=,is_fw=no,type=job,name=" + jobname + ",db=" + dbname + ",fw_type=form");
                gen.COM("clipb_open_job,job=" + jobname + ",update_clipboard=view_job");
                gen.COM("open_job,job=" + jobname);
                gen.COM("open_entity,job=" + jobname + ",type=matrix,name=matrix,iconic=no");
                gen.COM("matrix_add_step,job=" + jobname + ",matrix=matrix,step=test,col=1");
                gen.COM("matrix_add_layer,job=" + jobname + ",matrix=matrix,layer=lay1,row=1,context=board,type=signal,polarity=positive");
                gen.COM("matrix_add_layer,job=" + jobname + ",matrix=matrix,layer=lay2,row=2,context=board,type=signal,polarity=positive");

                string msg2 = "Now we'll open the newly created step...";
                Console.WriteLine(msg2);

                gen.PAUSE(msg2);

                gen.COM("open_entity,job=" + jobname + ",type=step,name=test,iconic=no");
                string group = gen.COMANS;
                gen.PAUSE("Group number is " + group);



                gen.COM("units,type=inch");
                gen.COM("display_layer,name=lay1,display=yes,number=1");
                gen.COM("work_layer,name=lay1");
                gen.COM("display_layer,name=lay2,display=yes,number=2");
                gen.COM("profile_rect,x1=0,y1=0,x2=5,y2=5");
                gen.COM("zoom_home");
                gen.COM("add_line,attributes=no,xs=0.6239601378,ys=0.5049917323,xe=4.6505824803,ye=4.5865225394,symbol=r40,polarity=positive");
                gen.COM("work_layer,name=lay2");
                gen.COM("add_line,attributes=no,xs=0.5049917323,ys=4.4675541339,xe=4.4584027559,ye=0.4409318898,symbol=r40,polarity=positive");

                gen.MOUSE("Click a point", "p");
                Console.WriteLine("Mouseans : " + gen.MOUSEANS);

                Console.WriteLine("Checking return of Genesis object...");
                Console.WriteLine("genStatus : " + Convert.ToString(gen.STATUS));
                Console.WriteLine("genComans : " + gen.COMANS);

                string msg3 = "Now we'll clean up...";
                Console.WriteLine(msg3);

                gen.PAUSE(msg3);


                gen.COM("check_inout,mode=in,type=job,job=" + jobname);
                gen.COM("close_job,job=" + jobname);
                gen.COM("close_form,job=" + jobname);
                gen.COM("close_flow,job=" + jobname);
                gen.COM("delete_entity,job=,type=job,name=" + jobname);


                Console.WriteLine("genStatus : " + Convert.ToString(gen.STATUS));
                Console.WriteLine("genComans : " + gen.COMANS);

                string msg4 = "Thats it...";
                Console.WriteLine(msg4);

                gen.PAUSE(msg4);

                Application.Exit();

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
public class Genesis
{

    public string prefix, lmc, msg;
    public string READANS, COMANS, PAUSANS, MOUSEANS;
    public int STATUS = 0;
    public System.IO.StreamReader conv;
    public System.IO.StreamReader @in;

    public Genesis()
    {
        //exe程序发送指令需要以这个字符串开头，genesis才会识别
        this.prefix = "@%#%@";
        this.blank();
        return;
    }


    public virtual void blank()
    {
        this.STATUS = 0;
        this.READANS = "";
        this.COMANS = "";
        this.PAUSANS = "";
        this.MOUSEANS = "";
        return;
    }
    /// <summary>
    /// 执行指令
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="arg"></param>
    /// <returns></returns>

    public virtual int sendCmd(string cmd, string arg)
    {


        this.blank();


        this.lmc = this.prefix + cmd + " " + arg + "\n";
        Console.Write(this.lmc);


        return 0;
    }

    /// <summary>
    /// 执行genesis2000 line mode command动作
    /// </summary>
    /// <param name="arg">指令</param>
    /// <returns></returns>
    public virtual int COM(string arg)
    {

        this.sendCmd("COM", arg);


        try
        {

            int.TryParse(Console.ReadLine(), out STATUS);
            this.COMANS = Console.ReadLine();
            this.READANS = this.COMANS;
        }
        catch (IOException e)
        {


            Console.WriteLine("IO Error: " + e.Message);
        }
        return this.STATUS;
    }

    /// <summary>
    ///      用于暂停当前运行程序，等待用户做其它动作后继续执行程序或无条件退出程序。
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public virtual int PAUSE(string msg)
    {

        this.sendCmd("PAUSE", msg);


        try
        {

            int.TryParse(Console.ReadLine(), out this.STATUS);
            this.READANS = Console.ReadLine();
            this.PAUSANS = Console.ReadLine();
        }
        catch (IOException e)
        {
            Console.WriteLine("IO Error: " + e.Message);
        }
        return this.STATUS;
    }
    /// <summary>
    /// 设置活动工作界面
    /// </summary>
    /// <param name="arg">指令</param>
    /// <returns></returns>
    public virtual int AUX(string arg)
    {
        this.sendCmd("AUX", msg);


        try
        {
            //this.STATUS  = this.in.readLine();
            this.STATUS = int.Parse(Console.ReadLine());
            this.COMANS = Console.ReadLine();
        }
        catch (IOException e)
        {
            Console.WriteLine("IO Error: " + e.Message);
        }
        return this.STATUS;
    }

    /// <summary>
    /// 用于获取鼠标点击位置坐标。
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="mode"></param>
    /// <returns></returns>


    public virtual int MOUSE(string msg, string mode)
    {

        this.sendCmd("MOUSE " + mode, msg);


        try
        {

            int.TryParse(Console.ReadLine(), out STATUS);
            this.READANS = Console.ReadLine();
            this.MOUSEANS = Console.ReadLine();
        }

        catch (IOException e)
        {
            Console.WriteLine("IO Error: " + e.Message);
        }
        return this.STATUS;
    }


    /// <summary>
    /// 指令的主要目的是当我们有些程序需要用超级用户才能执行的时候，我们用SU_ON打开超级用户，然后执行程序。
    /// </summary>
    /// <returns></returns>
    public virtual int SU_ON()
    {
        this.sendCmd("SU_ON", "");
        return 0;
    }
    /// <summary>
    /// 为退出由SU_ON打开的超级用户模式
    /// </summary>
    /// <returns></returns>
    public virtual int SU_OFF()
    {
        this.sendCmd("SU_OFF", "");
        return 0;
    }
    /// <summary>
    ///     VON主要用于取得VOF的结果，然后执行其它的一些指令
    /// </summary>
    /// <returns></returns>
    public virtual int VON()
    {
        this.sendCmd("VON", "");
        return 0;
    }
    /// <summary>
    /// 该指令主要用于设置执行有可能出现错误的动作，并利用status得到结果，并经常结合VON使用
    /// </summary>
    /// <returns></returns>

    public virtual int VOF()
    {
        this.sendCmd("VOF", "");
        return 0;
    }



}