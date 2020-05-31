using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Newtonsoft.Json;

namespace karpan
{
    public partial class Form1 : Form
    {
        public class GeoData
        {
            public double[] Coordinates { get; set; }
        }
        
        public class Object
        {
            public string Name { get; set; }
            public string TypeObject { get; set; }
            public string District { get; set; }
            public string Address { get; set; }
         
            public int SeatsCount { get; set; }
            public GeoData geoData { get; set; }

        }

        readonly List<Object> Elements;

        public Form1()
        {
            InitializeComponent();
            string path = "cafe.json";
            var encoder = Encoding.GetEncoding(1251);
            StreamReader r = new StreamReader(path, encoder);
            string json = r.ReadToEnd();
            Elements = JsonConvert.DeserializeObject<List<Object>>(json);
            MessageBox.Show(
                    "Данные успешно загружены",
                    "Загрузка данных",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            textBox2.Text = "0";
            r.Dispose();
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            // Настройки для компонента GMap
            gmap.Bearing = 0;
            // Перетаскивание правой кнопки мыши
            gmap.CanDragMap = true;
            // Перетаскивание карты левой кнопкой мыши
            gmap.DragButton = MouseButtons.Left;

            gmap.GrayScaleMode = true;

            // Все маркеры будут показаны
            gmap.MarkersEnabled = true;
            // Максимальное приближение
            gmap.MaxZoom = 18;
            // Минимальное приближение
            gmap.MinZoom = 2;
            // Курсор мыши в центр карты
            gmap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;

            // Отключение нигативного режима
            gmap.NegativeMode = false;
            // Разрешение полигонов
            gmap.PolygonsEnabled = true;
            // Разрешение маршрутов
            gmap.RoutesEnabled = true;
            // Скрытие внешней сетки карты
            gmap.ShowTileGridLines = false;
            // При загрузке 10-кратное увеличение
            gmap.Zoom = 10;

            // Чья карта используется
            gmap.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;

            // Загрузка этой точки на карте
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gmap.Position = new GMap.NET.PointLatLng(55.7522200, 37.6155600);

            gmap.Overlays.Add(new GMapOverlay());
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if ((!Char.IsDigit(number))&& (!char.IsControl(number)))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int flag = 0;
            gmap.Zoom = 13;

            if (textBox1.Text == "")
            {
                MessageBox.Show(
                    "Введите район", 
                    "Ошибка", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            if ((checkBox1.Checked != true) && (checkBox2.Checked != true) && (checkBox3.Checked != true))
            {
                MessageBox.Show(
                    "Выберите тип заведения",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            foreach (var item in Elements)
            {
                if (checkBox1.Checked == true)
                {
                    if ((item.TypeObject == "кафе") && (item.SeatsCount > int.Parse(textBox2.Text)) && (item.District == textBox1.Text))
                    {
                        double lat = item.geoData.Coordinates[1];
                        double lon = item.geoData.Coordinates[0];
                        GMarkerGoogle mark = new GMarkerGoogle(new PointLatLng(lat, lon), GMarkerGoogleType.blue_small)
                        {
                            ToolTipText = item.Name
                        };

                        gmap.Overlays[0].Markers.Add(mark);
                        richTextBox1.Text += item.Name + "\n" + item.Address + "\n" + "Кафе" + "\n" + "Количество мест: " + item.SeatsCount + "\n" + "\n";

                        if (flag == 0)
                        {
                            flag = 1;
                            gmap.Position = new GMap.NET.PointLatLng(item.geoData.Coordinates[1], item.geoData.Coordinates[0]);
                        }
                    }
                }

                if (checkBox2.Checked == true)
                {
                    if ((item.TypeObject == "ресторан") && (item.SeatsCount > int.Parse(textBox2.Text)) && (item.District == textBox1.Text))
                    {
                        double lat = item.geoData.Coordinates[1];
                        double lon = item.geoData.Coordinates[0];
                        GMarkerGoogle mark = new GMarkerGoogle(new PointLatLng(lat, lon), GMarkerGoogleType.yellow_small)
                        {
                            ToolTipText = item.Name
                        };

                        gmap.Overlays[0].Markers.Add(mark);
                        richTextBox1.Text += item.Name + "\n" + item.Address + "\n" + "Ресторан" + "\n" + "Количество мест: " + item.SeatsCount + "\n" + "\n";

                        if (flag == 0)
                        {
                            flag = 1;
                            gmap.Position = new GMap.NET.PointLatLng(item.geoData.Coordinates[1], item.geoData.Coordinates[0]);
                        }
                    }
                }

                if (checkBox3.Checked == true)
                {
                    if ((item.TypeObject == "столовая") && (item.SeatsCount > int.Parse(textBox2.Text)) && (item.District == textBox1.Text))
                    {
                        double lat = item.geoData.Coordinates[1];
                        double lon = item.geoData.Coordinates[0];
                        GMarkerGoogle mark = new GMarkerGoogle(new PointLatLng(lat, lon), GMarkerGoogleType.white_small)
                        {
                            ToolTipText = item.Name
                        };

                        gmap.Overlays[0].Markers.Add(mark);
                        richTextBox1.Text += item.Name + "\n" + item.Address + "\n" + "Столовая" + "\n" + "Количество мест: " + item.SeatsCount + "\n" + "\n";

                        if (flag == 0)
                        {
                            flag = 1;
                            gmap.Position = new GMap.NET.PointLatLng(item.geoData.Coordinates[1], item.geoData.Coordinates[0]);
                        }
                    }
                }
            }

            if (richTextBox1.Text == "")
            {
                richTextBox1.Text = "Список пуст";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gmap.Overlays[0].Clear();
            richTextBox1.Text = "";
        }
    }
}
