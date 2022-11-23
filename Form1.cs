using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CellularAutomaton
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void timer_Tick(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Field field = new Field(pictureBox, 50, 50);
            field.AddAnt(new Ant(), 0);
            field.AddAnt(new Ant(), 2);
            //field.AddAnt(new Ant(), 2);
            //field.AddAnt(new Ant(), 3);
            field.Start();
        }
    }

    public class Ant
    {
        public int x;
        public int y;
        public int direction;

        public Ant() {
            
        }

        public void RotateRight()
        {
            direction += 1;
            if (direction > 3) direction = 0;
        }
        public void RotateLeft()
        {
            direction -= 1;
            if (direction < 0) direction = 3;
        }
    }
    public class Field
    {
        public int width, height;
        Timer t = new Timer();
        private int[,] matrix;
        int generation = 0;
        int cell_size = 20;
        Bitmap bmp;
        Graphics cg;
        List<Ant> ants = new List<Ant>();
        PictureBox pictureBox;

        public Field(PictureBox _pictureBox, int w, int h)
        {
            width = w;
            height = h;
            matrix = new int[w, h];
            pictureBox = _pictureBox;
            FillMatrix();
            DrawMatrix(_pictureBox, w, h);

            t.Interval = 1;
            t.Tick += new EventHandler(next_step);
        }

        //public void AddAnt(Ant new_ant, int x, int y, int direction)
        //{
        //    ant = new_ant;
        //    ant.x = x;
        //    ant.y = y;
        //    ant.direction = direction;
        //}

        public void AddAnt(Ant ant, int direction)
        {
            Ant _ant = ant;
            _ant.x = width / 2 + 1;
            _ant.y = height / 2 + 1;
            _ant.direction = direction;
            ants.Add(_ant);
        }

        public void Start()
        {
            t.Start();
        }

        private void FillMatrix()
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    matrix[i, j] = 1;
        }

        private void DrawMatrix(PictureBox pictureBox, int w, int h)
        {
            bmp = new Bitmap(w * cell_size + 1, h * cell_size + 1);
            cg = Graphics.FromImage(bmp);
            cg.Clear(Color.Black);
            //for (int i = 0; i < width; i++)
            //    for (int j = 0; j < height; j++)
            //        cg.DrawRectangle(new Pen(Color.Black, 1f), i * cell_size, j * cell_size, (i + 1) * cell_size, (j + 1) * cell_size);

            pictureBox.Image = bmp;
        }

        private void next_step(object sender, EventArgs e)
        {
            generation++;
            foreach (Ant ant in ants)
            {
                switch (ant.direction)
                {
                    case 0:
                        ant.y -= 1;
                        if (ant.y < 0) ant.y = height - 1;
                        break;
                    case 1:
                        ant.x += 1;
                        if (ant.x > width - 1) ant.x = 0;
                        break;
                    case 2:
                        ant.y += 1;
                        if (ant.y > height - 1) ant.y = 0;
                        break;
                    case 3:
                        ant.x -= 1;
                        if (ant.x < 0) ant.x = width - 1;
                        break;
                }

                if (matrix[ant.x, ant.y] == 0)
                {
                    matrix[ant.x, ant.y] = 1;
                    cg.FillRectangle(new SolidBrush(Color.Black), ant.x * cell_size, ant.y * cell_size, cell_size, cell_size);
                    ant.RotateRight();
                }
                else if (matrix[ant.x, ant.y] == 1)
                {
                    matrix[ant.x, ant.y] = 0;
                    cg.FillRectangle(new SolidBrush(Color.White), ant.x * cell_size, ant.y * cell_size, cell_size, cell_size);
                    ant.RotateLeft();
                }
                pictureBox.Image = bmp;

            }
            //Redraw();
        }

        private void Redraw()
        {
            bmp = new Bitmap(width * cell_size + 1, height * cell_size + 1);
            cg = Graphics.FromImage(bmp);
            //cg.Clear(Color.White);
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    cg.DrawRectangle(new Pen(Color.Black, 1f), i * cell_size, j * cell_size, (i + 1) * cell_size, (j + 1) * cell_size);

            for (int i = 1; i < width; i++)
                for (int j = 1; j < height; j++)
                    //if (matrix[i, j] == 0)
                    //    cg.FillRectangle(new SolidBrush(Color.White), new RectangleF(i * cell_size, j * cell_size, (i + 1) * cell_size, (j + 1) * cell_size));
                    //else if (matrix[i, j] == 1)
                    //    cg.FillRectangle(new SolidBrush(Color.Black), new RectangleF(i * cell_size, j * cell_size, (i + 1) * cell_size, (j + 1) * cell_size));
            if (matrix[i, j] == 0)
                cg.FillRectangle(new SolidBrush(Color.White), i * cell_size, j * cell_size, cell_size, cell_size);
            else if (matrix[i, j] == 1)
                cg.FillRectangle(new SolidBrush(Color.Black), i * cell_size, j * cell_size, cell_size, cell_size);

            pictureBox.Image = bmp;
        }
    }
}
