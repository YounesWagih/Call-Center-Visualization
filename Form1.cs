using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Call_Center
{
    public partial class Form1 : Form
    {
        Queue<Person> clients_queue=new Queue<Person>();
        Queue<Person> clients_finish=new Queue<Person>();
        Label tmp;
        int timelft=5;
        int endHeigh=287;
        int ClintesID = 1;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Label.CheckForIllegalCrossThreadCalls = false;
        }
        private async void GenerateBtn_Click(object sender, EventArgs e)
        {
            if (clients_queue.Count==0)
            {
                int num = int.Parse(comboBox1.Text);
                for (int i = 1,Label_location=287; i <= num; i++,Label_location-=31)
                {
                    generalclints(Label_location);
                    Task wt = Task.Run(async delegate { await Task.Delay(500); });
                    await wt;
                    if (i==1)
                    {
                        Task s = new Task(start);
                        s.Start();
                    }
                }
            }
        }

        private void generalclints(int label_location)
        {
            int i = ClintesID;
            Person l = new Person();
            l.lbl.Text =$"Client  | {i} | {DateTime.Now.ToString("mm:ss")} |  ";
            l.id=i;
            l.enterTime = DateTime.Now.ToString("mm:ss");
            l.lbl.AutoSize = true;
            l.lbl.BackColor = System.Drawing.Color.CornflowerBlue;
            l.lbl.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            l.lbl.ForeColor = System.Drawing.Color.White;
            l.lbl.Location = new System.Drawing.Point(12, label_location);
            l.lbl.Name = "label"+i;
            l.lbl.Size = new System.Drawing.Size(109, 31);
            l.lbl.Click += (sender, e) => evnt (l, sender, e);
            this.Controls.Add(l.lbl);
            l.lbl.BringToFront();
            clients_queue.Enqueue(l);
            ClintesID++;
        }

        private void evnt(Person l, object sender, EventArgs e)
        {
            MessageBox.Show($" id : {l.id} \n Enter Time : {l.enterTime} \n Exit Time : {l.ExitTime} ");
        }

        private async void start()
        {
            while (clients_queue.Count > 0)
            {
                Task sa = new Task(label_move);
                //Task tim = new Task(CountDown);
                //tim.Start();
                sa.Start();
                Task wt = Task.Run(async delegate { await Task.Delay(5000);});
                await wt;
                clients_queue.First().lbl.Text += DateTime.Now.ToString("mm:ss");
                clients_queue.First().ExitTime = DateTime.Now.ToString("mm:ss");
                clients_finish.Enqueue(clients_queue.First());
                label_Endmove(clients_queue.Dequeue().lbl);
            }
        }

        private async void label_move()
        {
            var tmp = clients_queue.First().lbl;
            while (tmp.Location.X < 323)
            {
                tmp.Invoke((MethodInvoker)(() => tmp.Location = new Point(tmp.Location.X + 1, tmp.Location.Y)));
            }
            while (tmp.Location.Y < 285)
            {
                tmp.Invoke((MethodInvoker)(() => tmp.Location = new Point(tmp.Location.X, tmp.Location.Y + 1)));
            }
        }
        private void CountDown()
        {
            while (timelft > 0)
            {
                Thread.Sleep(1000);
                timelft--;
                timeLB.Text = timelft + " seconds";
            }
                timelft = 5;
                timeLB.Text = timelft + " seconds";

        }

        private void label_Endmove(Label tmp)
        {
            while (tmp.Location.X < 673)
            {
                tmp.Location = new Point(tmp.Location.X + 1, tmp.Location.Y);
            }
            while (tmp.Location.Y > endHeigh)
            {
                tmp.Location = new Point(tmp.Location.X, tmp.Location.Y - 1);
            }
            endHeigh -= 30;
        }


        private void ClearBtn_Click(object sender, EventArgs e)
        {
            foreach (var client in clients_finish)
            {
                this.Controls.Remove(client.lbl);
            }
            clients_finish.Clear();
        }
    }
}