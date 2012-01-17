using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _2lb_this
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();
        }

        private void Esc(object sender, PreviewKeyDownEventArgs e)
        {
            Close();
        }
        
        const int arrayLength = 5;
        Point[] polygonPoints = new Point[arrayLength]; //точки пятиугольника
        bool startGraphics = false;

        int DrawSomeShapes()
        {
            Graphics g = this.CreateGraphics(); //подготовка области рисования на форме
            Pen bluePen = new Pen(Color.Blue, 4); //подготавливаем перо, рисующее красную линию толщиной 4 пикселя
            Pen whitePen = new Pen(Color.White, 2); //подготавливаем перо, рисующее булую линию толщиной 2 пикселя
            if (!startGraphics)
            {
                button1.Visible = false; //скрыть кнопку
                for (int i = 0; i < 65; i++) //анимация
                {
                    polygonPoints[0] = new Point(63 + i, 233 - i);
                    polygonPoints[1] = new Point(20 + (int)(i * 1.5), 106 + i);
                    polygonPoints[2] = new Point(130, 20 + i * 2);
                    polygonPoints[3] = new Point(240 - (int)(i * 1.5), 106 + i);
                    polygonPoints[4] = new Point(200 - i, 233 - i);
                    Update();
                    g.DrawPolygon(bluePen, polygonPoints); //нарисовать пятиугольник
                    System.Threading.Thread.Sleep(40);
                }
                startGraphics = false; //не разрешить больше нажимать кнопку
            }
            g.DrawEllipse(whitePen, 100, 100, 25, 25); //большой кржок
            g.DrawEllipse(whitePen, 140, 112, 10, 10); //маленький кружок
            g.Dispose(); //очистка
            return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawSomeShapes();
        }
    }
}
