namespace Microsoft.Samples.Kinect.DepthBasics
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;
    using System.Drawing;
    using System.Windows.Threading;
    public partial class MainWindow : Window
    {

        private KinectSensor sensor;
        private WriteableBitmap colorBitmap;
        private byte[] pixelesColores;
        private BitmapEncoder encoder;
        private string path;
        private string misFotos;
        private string hora;
        private ProcesadorDeImagen procesador;
        private BitmapImage imagenConFiltro;
        private DispatcherTimer timer;
        private DepthImagePixel[] pixelesProfundidad;

        private int numero = 1;
        private bool visible = false;
        private bool iniciado = false;

        public MainWindow()
        {
            InitializeComponent();
            procesador = new ProcesadorDeImagen();
            misFotos = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\industrial";

            hora = DateTime.Now.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);
        }

        private void iniciarVentana(object sender, RoutedEventArgs e)
        {
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (null != this.sensor)
            {
                this.sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

                this.pixelesColores = new byte[this.sensor.ColorStream.FramePixelDataLength];

                this.colorBitmap = new WriteableBitmap(this.sensor.ColorStream.FrameWidth,
                    this.sensor.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                this.Video.Source = this.colorBitmap;

                this.sensor.ColorFrameReady += this.colorFrameListener;

                try
                {
                    this.sensor.Start();
                }
                catch (IOException)
                {
                    this.sensor = null;
                }
            }
        }

        private void cerrarVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != this.sensor)
            {
                this.sensor.Stop();
            }
        }

        private void colorFrameListener(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame frameColores = e.OpenColorImageFrame())
            {
                if (frameColores != null)
                {
                    frameColores.CopyPixelDataTo(this.pixelesColores);

                    this.colorBitmap.WritePixels(
                        new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                        this.pixelesColores,
                        this.colorBitmap.PixelWidth * sizeof(int), 0);
                }
            }
        }

        private void profundidadFrameListener(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (DepthImageFrame frameProfundidad = e.OpenDepthImageFrame())
            {
                if (frameProfundidad != null)
                {
                    frameProfundidad.CopyDepthImagePixelDataTo(this.pixelesProfundidad);

                    int min = frameProfundidad.MinDepth;
                    int max = frameProfundidad.MaxDepth;

                    int colorPixelIndex = 0;

                    for (int i = 0; i < this.pixelesProfundidad.Length; ++i)
                    {

                        short profundidad = pixelesProfundidad[i].Depth;

                        byte intensidad = (byte)(profundidad >= min && profundidad <= max ? profundidad : 0);

                        this.pixelesColores[colorPixelIndex++] = intensidad;
                        this.pixelesColores[colorPixelIndex++] = intensidad;
                        this.pixelesColores[colorPixelIndex++] = intensidad;

                        ++colorPixelIndex;
                    }

                    this.colorBitmap.WritePixels(new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                        this.pixelesColores, this.colorBitmap.PixelWidth * sizeof(int), 0);
                }
            }
        }

        private void listenerScreenshot(object sender, RoutedEventArgs e)
        {
            this.encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(this.colorBitmap));
            hora = DateTime.Now.ToString("hh'-'mm'-'ss", CultureInfo.CurrentUICulture.DateTimeFormat);
            path = Path.Combine(misFotos, "Screenshot" + hora + ".bmp");
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    encoder.Save(fs);
                }
                this.Imagen.Source = new BitmapImage(new Uri(path));
            }
            catch (IOException)
            {
                MessageBox.Show("La ruta elegida no es una ruta disponible");
            }
        }

        private void listenerFiltroCanny(object sender, RoutedEventArgs e)
        {
            if (this.imagenConFiltro != null)
            {
                Bitmap bitmap = this.procesador.bitmapFromBitmapImage(this.imagenConFiltro);
                this.imagenConFiltro = this.procesador.aplicarFiltroCanny(bitmap, misFotos, hora);
                this.Imagen.Source = this.imagenConFiltro;
            }
            else
            {
                MessageBox.Show("Primero debe aplicar un efecto");
            }
        }

        private void listenerFiltroSobel(object sender, RoutedEventArgs e)
        {
            if (this.imagenConFiltro != null)
            {
                Bitmap bitmap = this.procesador.bitmapFromBitmapImage(this.imagenConFiltro);
                this.imagenConFiltro = this.procesador.aplicarFiltroSobel(bitmap, misFotos, hora);
                this.Imagen.Source = this.imagenConFiltro;
            }
            else
            {
                MessageBox.Show("Primero debe aplicar un efecto");
            }
        }

        private void listenerDetectarFormas(object sender, RoutedEventArgs e)
        {

            if (this.imagenConFiltro != null)
            {
                Bitmap bitmap = this.procesador.bitmapFromBitmapImage(this.imagenConFiltro);
                this.Imagen.Source = this.procesador.detectarCuadrilateros(bitmap, misFotos, hora);
            }
            else
            {
                MessageBox.Show("Debes tener una imagen con un filtro aplicado");
            }
        }

        private void iniciarTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(2000);
            timer.Tick += eventoARealizar;
            timer.Start();
        }

        private void eventoARealizar(object source, EventArgs e)
        {
            path = Path.Combine(misFotos, "timer/Foto" + numero + ".bmp");
            BitmapImage bitmapImageTimer = new BitmapImage(new Uri(path));
            Console.WriteLine("paso");
            this.Imagen.Source = bitmapImageTimer;

            if (numero == 4)
            {
                timer.Stop();
            }
            numero++;
        }

        private void listenerPanelControl(object sender, RoutedEventArgs e)
        {
            if (!visible)
            {
                this.iniciarProcesamiento.Visibility = Visibility.Visible;
                this.comboBoxEfectos.Visibility = Visibility.Hidden;
                this.filtroCanny.Visibility = Visibility.Hidden;
                this.filtroSobel.Visibility = Visibility.Hidden;
                this.detectorCuadrilateros.Visibility = Visibility.Hidden;
                this.screenShot.Visibility = Visibility.Hidden;
                visible = true;
            }
            else
            {
                this.iniciarProcesamiento.Visibility = Visibility.Hidden;
                this.comboBoxEfectos.Visibility = Visibility.Visible;
                this.filtroCanny.Visibility = Visibility.Visible;
                this.filtroSobel.Visibility = Visibility.Visible;
                this.detectorCuadrilateros.Visibility = Visibility.Visible;
                this.screenShot.Visibility = Visibility.Visible;
                visible = false;
            }
        }

        private void listenerIniciarProcesamiento(object sender, RoutedEventArgs e)
        {
            if (iniciado)
            {
                this.iniciarProcesamiento.Content = "Iniciar procesamiento";
                iniciado = false;
                timer.Stop();
            }
            else
            {
                this.iniciarProcesamiento.Content = "Parar procesamiento";
                iniciado = true;
                iniciarTimer();
            }
        }

        private void comboBoxEfectos_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            path = Path.Combine(misFotos, "Screenshot11-36-23.bmp");
            int efecto = comboBoxEfectos.SelectedIndex;

            switch (efecto)
            {
                case 0:
                    this.imagenConFiltro = this.procesador.Bitonal(path, misFotos, hora, System.Drawing.Color.Black, System.Drawing.Color.Green, 250);
                    this.Imagen.Source = this.imagenConFiltro;
                    break;

                case 1:
                    this.imagenConFiltro = this.procesador.gamma(path, misFotos, hora, 3, 3, 3);
                    this.Imagen.Source = this.imagenConFiltro;
                    break;

                case 2:
                    this.imagenConFiltro = this.procesador.aplicarGrises(path, misFotos, hora);
                    this.Imagen.Source = this.imagenConFiltro;
                    break;

                case 3:
                    this.imagenConFiltro = this.procesador.aplicarContraste(path, misFotos, hora, 30);
                    this.Imagen.Source = this.imagenConFiltro;
                    break;

                    /**
                    case :
                        this.imagenConFiltro = this.procesador.aplicarHough(path, misFotos);
                        this.Imagen.Source = this.imagenConFiltro;
                        break;
                    */
            }
        }

        private void profundidad_Checked(object sender, RoutedEventArgs e)
        {
            if (null != this.sensor)
            {
                if (this.profundidad.IsChecked == false)
                {
                    this.sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

                    this.pixelesColores = new byte[this.sensor.ColorStream.FramePixelDataLength];

                    this.colorBitmap = new WriteableBitmap(this.sensor.ColorStream.FrameWidth,
                        this.sensor.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                    this.Video.Source = this.colorBitmap;

                    this.sensor.ColorFrameReady += this.colorFrameListener;
                }
                else
                {
                    this.sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);

                    this.pixelesProfundidad = new DepthImagePixel[this.sensor.DepthStream.FramePixelDataLength];

                    this.pixelesColores = new byte[this.sensor.DepthStream.FramePixelDataLength * sizeof(int)];

                    this.colorBitmap = new WriteableBitmap(this.sensor.DepthStream.FrameWidth, this.sensor.DepthStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                    this.Video.Source = this.colorBitmap;

                    this.sensor.DepthFrameReady += this.profundidadFrameListener;
                }
            }
        }
    }
}
