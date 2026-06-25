namespace OffloadingTasks
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Console.WriteLine("Simple button click handler using Multithreading!!!\n");
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(()=> ShowMessage("First Message", 3000));
            thread.Start();

            //ShowMessage("First Message", 3000);

            //Thread.Sleep(2000);
            //lblMessage.Text = "First Message";
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => ShowMessage("Second Message", 4000));
             thread.Start();

            //ShowMessage("Second Message", 5000);

            //Thread.Sleep(3000);
            //lblMessage.Text = "Second Message";
        }

        private void ShowMessage(string message, int delay)
        {
            Thread.Sleep(delay);

            if (lblMessage.InvokeRequired)
            {
                lblMessage.Invoke((MethodInvoker)(() => lblMessage.Text = message));
            }
            else
            {
                lblMessage.Text = message;
            }
        }
    }
}
