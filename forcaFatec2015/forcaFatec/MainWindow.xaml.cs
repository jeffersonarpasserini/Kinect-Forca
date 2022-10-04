using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;
using Microsoft.Kinect.Toolkit.Interaction;


namespace forcaFatec
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Verificar quantos kinects estão conectados
        KinectSensorChooser meuKinect;

        public String dir_sistema = @"C:\Users\jeffersonpasserini\Downloads\forcaFatec\forcaFatec";
        //public String dir_sistema = Directory.GetCurrentDirectory();
        public string dirSons;
        public String somErro;
        public String somAcerto;
        public String somInicio;

        public string[] aPalavras = new string[30];
        public string[] aPortugues = new string[30];
        public string[] aDicas = new string[30];
        public string[] aArq_Imagem = new string[30];
        public string[] aPronuncia = new string[30];
        public string[,] aLetras = new string[30, 6];

        public int nroSorteado;
        public string resultadoLetra0;
        public string resultadoLetra1;
        public string resultadoLetra2;
        public string resultadoLetra3;
        public string resultadoLetra4;
        public string resultadoLetra5;
        public int contaErros=0;


        public string letra_escolhida;
        public string som_escolhido;
        public string img_escolhida;

        public MainWindow()
        {
            InitializeComponent();   
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dirSons = dir_sistema + @"\Sounds\";
            somErro = dirSons + "erro.wav";
            //somErro = @"\Users\Jefferson\Documents\Visual Studio 2013\Projects\forcaFatec\forcaFatec\Sounds\erro.wav";
            somAcerto = dirSons + "acerto.wav";
            //somAcerto = @"\Users\Jefferson\Documents\Visual Studio 2013\Projects\forcaFatec\forcaFatec\Sounds\acerto.wav";
            somInicio = dirSons + "inicio.wav";
            //somInicio = @"\Users\Jefferson\Documents\Visual Studio 2013\Projects\forcaFatec\forcaFatec\Sounds\inicio.wav";

            //tratando a conexão e desconexão do kinect
            meuKinect = new KinectSensorChooser();
            //vai enviar a mensagem
            meuKinect.KinectChanged += meuKinect_KinectChanged;
            //se estiver conectado vai mandar para variavel
            sensorChooserUI.KinectSensorChooser = meuKinect;
            //vai iniciar o kinect
            meuKinect.Start();

            defineBases();
            sorteiaPalavra();
            lblNroErros.Content = contaErros;
        }

        void meuKinect_KinectChanged(object sender, KinectChangedEventArgs e)
        {
            bool error = true;

            if (e.OldSensor == null)
            {
                try
                {
                    e.OldSensor.DepthStream.Disable();
                    e.OldSensor.SkeletonStream.Disable();
                }
                catch (Exception)
                {
                    error = true;
                }
            }

            if (e.NewSensor == null)
                return;

            try
            {
                //kinect xbox360
                e.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                e.NewSensor.SkeletonStream.Enable();

                try
                {
                    //kinect windows
                    e.NewSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
                    e.NewSensor.DepthStream.Range = DepthRange.Near;
                    e.NewSensor.SkeletonStream.EnableTrackingInNearRange = true;
                }
                catch (InvalidOperationException)
                {
                    e.NewSensor.DepthStream.Range = DepthRange.Default;
                    e.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
                }
            }
            catch (InvalidOperationException)
            {
                error = true;
            }

            //configurar a região que vamos controlar com o cursor
            zonaCursor.KinectSensor = e.NewSensor;

        }

        #region "*---------------Base de Palavaras---------------*"
        public void defineBases()
        {
            String arquivo = dir_sistema+@"\Images\";
            String arquivo2 = dir_sistema + @"\Sounds\";

            aPalavras[0] = "grape";
            aPortugues[0] = "uva";
            aDicas[0] = "fruta";
            //aArq_Imagem[0] = arquivo + "wgrape.png";
            aArq_Imagem[0] = "/Images/wgrape.png";
            aPronuncia[0] = "pgrape.wav";
            aLetras[0, 0] = "g";
            aLetras[0, 1] = "r";
            aLetras[0, 2] = "a";
            aLetras[0, 3] = "p";
            aLetras[0, 4] = "e";
            aLetras[0, 5] = "";

            aPalavras[1] = "apple";
            aPortugues[1] = "maçã";
            aDicas[1] = "fruta";
            //aArq_Imagem[1] = arquivo+"wapple.png";
            aArq_Imagem[1] = "/Images/wapple.png";
            aPronuncia[1] = "papple.wav";
            aLetras[1, 0] = "a";
            aLetras[1, 1] = "p";
            aLetras[1, 2] = "p";
            aLetras[1, 3] = "l";
            aLetras[1, 4] = "e";
            aLetras[1, 5] = "";

            aPalavras[2] = "orange";
            aPortugues[2] = "laranja";
            aDicas[2] = "fruta";
            //aArq_Imagem[2] = arquivo + "worange.png";
            aArq_Imagem[2] = "/Images/worange.png";
            aPronuncia[2] = "porange.wav";
            aLetras[2, 0] = "o";
            aLetras[2, 1] = "r";
            aLetras[2, 2] = "a";
            aLetras[2, 3] = "n";
            aLetras[2, 4] = "g";
            aLetras[2, 5] = "e";

            aPalavras[3] = "watch";
            aPortugues[3] = "relógio";
            aDicas[3] = "acessório";
            aArq_Imagem[3] = "/Images/wwatch.png";
            aPronuncia[3] = "pwatch.wav";
            aLetras[3, 0] = "w";
            aLetras[3, 1] = "a";
            aLetras[3, 2] = "t";
            aLetras[3, 3] = "c";
            aLetras[3, 4] = "h";
            aLetras[3, 5] = "";

            aPalavras[4] = "book";
            aPortugues[4] = "livro";
            aDicas[4] = "leitura";
            aArq_Imagem[4] = "/Images/wbook.png";
            aPronuncia[4] = "pbook.wav";
            aLetras[4, 0] = "b";
            aLetras[4, 1] = "o";
            aLetras[4, 2] = "o";
            aLetras[4, 3] = "k";
            aLetras[4, 4] = "";
            aLetras[4, 5] = "";

            aPalavras[5] = "wallet";
            aPortugues[5] = "carteira";
            aDicas[5] = "dinheiro";
            aArq_Imagem[5] = "/Images/wwallet.png";
            aPronuncia[5] = "pwallet.wav";
            aLetras[5, 0] = "w";
            aLetras[5, 1] = "a";
            aLetras[5, 2] = "l";
            aLetras[5, 3] = "l";
            aLetras[5, 4] = "e";
            aLetras[5, 5] = "t";

            aPalavras[6] = "tall";
            aPortugues[6] = "alto";
            aDicas[6] = "adjetivo";
            aArq_Imagem[6] = "/Images/wtall.png";
            aPronuncia[6] = "ptall.wav";
            aLetras[6, 0] = "t";
            aLetras[6, 1] = "a";
            aLetras[6, 2] = "l";
            aLetras[6, 3] = "l";
            aLetras[6, 4] = "";
            aLetras[6, 5] = "";

            aPalavras[7] = "eight";
            aPortugues[7] = "oito";
            aDicas[7] = "números";
            aArq_Imagem[7] = "/Images/weight.png";
            aPronuncia[7] = "peight.wav";
            aLetras[7, 0] = "e";
            aLetras[7, 1] = "i";
            aLetras[7, 2] = "g";
            aLetras[7, 3] = "h";
            aLetras[7, 4] = "t";
            aLetras[7, 5] = "";

            aPalavras[8] = "three";
            aPortugues[8] = "três";
            aDicas[8] = "números";
            aArq_Imagem[8] = "/Images/wthree.png";
            aPronuncia[8] = "pthree.wav";
            aLetras[8, 0] = "t";
            aLetras[8, 1] = "h";
            aLetras[8, 2] = "r";
            aLetras[8, 3] = "e";
            aLetras[8, 4] = "e";
            aLetras[8, 5] = "";

            aPalavras[9] = "tree";
            aPortugues[9] = "árvore";
            aDicas[9] = "natureza";
            aArq_Imagem[9] = "/Images/wtree.png";
            aPronuncia[9] = "ptree.wav";
            aLetras[9, 0] = "t";
            aLetras[9, 1] = "r";
            aLetras[9, 2] = "e";
            aLetras[9, 3] = "e";
            aLetras[9, 4] = "";
            aLetras[9, 5] = "";

            aPalavras[10] = "chair";
            aPortugues[10] = "cadeira";
            aDicas[10] = "móveis";
            aArq_Imagem[10] = "/Images/wchair.png";
            aPronuncia[10] = "pchair.wav";
            aLetras[10, 0] = "c";
            aLetras[10, 1] = "h";
            aLetras[10, 2] = "a";
            aLetras[10, 3] = "i";
            aLetras[10, 4] = "r";
            aLetras[10, 5] = "";

            aPalavras[11] = "dress";
            aPortugues[11] = "vestido";
            aDicas[11] = "vestuário";
            aArq_Imagem[11] = "/Images/wdress.png";
            aPronuncia[11] = "pdress.wav";
            aLetras[11, 0] = "d";
            aLetras[11, 1] = "r";
            aLetras[11, 2] = "e";
            aLetras[11, 3] = "s";
            aLetras[11, 4] = "s";
            aLetras[11, 5] = "";

            aPalavras[12] = "socks";
            aPortugues[12] = "meias";
            aDicas[12] = "vestuário";
            aArq_Imagem[12] = "/Images/wsocks.png";
            aPronuncia[12] = "psocks.wav";
            aLetras[12, 0] = "s";
            aLetras[12, 1] = "o";
            aLetras[12, 2] = "c";
            aLetras[12, 3] = "k";
            aLetras[12, 4] = "s";
            aLetras[12, 5] = "";

            aPalavras[13] = "pants";
            aPortugues[13] = "calça";
            aDicas[13] = "vestuário";
            aArq_Imagem[13] = "/Images/wpants.png";
            aPronuncia[13] = "ppants.wav";
            aLetras[13, 0] = "p";
            aLetras[13, 1] = "a";
            aLetras[13, 2] = "n";
            aLetras[13, 3] = "t";
            aLetras[13, 4] = "s";
            aLetras[13, 5] = "";

            aPalavras[14] = "summer";
            aPortugues[14] = "verão";
            aDicas[14] = "estação";
            aArq_Imagem[14] = "/Images/wsummer.png";
            aPronuncia[14] = "psummer.wav";
            aLetras[14, 0] = "s";
            aLetras[14, 1] = "u";
            aLetras[14, 2] = "m";
            aLetras[14, 3] = "m";
            aLetras[14, 4] = "e";
            aLetras[14, 5] = "r";

            aPalavras[15] = "spring";
            aPortugues[15] = "primavera";
            aDicas[15] = "estação";
            aArq_Imagem[15] = "/Images/wspring.png";
            aPronuncia[15] = "pspring.wav";
            aLetras[15, 0] = "s";
            aLetras[15, 1] = "p";
            aLetras[15, 2] = "r";
            aLetras[15, 3] = "i";
            aLetras[15, 4] = "n";
            aLetras[15, 5] = "g";

            aPalavras[16] = "winter";
            aPortugues[16] = "inverno";
            aDicas[16] = "estação";
            aArq_Imagem[16] = "/Images/wwinter.png";
            aPronuncia[16] = "pwinter.wav";
            aLetras[16, 0] = "w";
            aLetras[16, 1] = "i";
            aLetras[16, 2] = "n";
            aLetras[16, 3] = "t";
            aLetras[16, 4] = "e";
            aLetras[16, 5] = "r";

            aPalavras[17] = "autumn";
            aPortugues[17] = "outono";
            aDicas[17] = "estação";
            aArq_Imagem[17] = "/Images/wautumn.png";
            aPronuncia[17] = "pautumn.wav";
            aLetras[17, 0] = "a";
            aLetras[17, 1] = "u";
            aLetras[17, 2] = "t";
            aLetras[17, 3] = "u";
            aLetras[17, 4] = "m";
            aLetras[17, 5] = "n";

            aPalavras[18] = "laptop";
            aPortugues[18] = "pc portátil";
            aDicas[18] = "eletrônico";
            aArq_Imagem[18] = "/Images/wlaptop.png";
            aPronuncia[18] = "plaptop.wav";
            aLetras[18, 0] = "l";
            aLetras[18, 1] = "a";
            aLetras[18, 2] = "p";
            aLetras[18, 3] = "t";
            aLetras[18, 4] = "o";
            aLetras[18, 5] = "p";

            aPalavras[19] = "subway";
            aPortugues[19] = "metrô";
            aDicas[19] = "transporte";
            aArq_Imagem[19] = "/Images/wsubway.png";
            aPronuncia[19] = "psubway.wav";
            aLetras[19, 0] = "s";
            aLetras[19, 1] = "u";
            aLetras[19, 2] = "b";
            aLetras[19, 3] = "w";
            aLetras[19, 4] = "a";
            aLetras[19, 5] = "y";

            aPalavras[20] = "cab";
            aPortugues[20] = "táxi inglês";
            aDicas[20] = "transporte";
            aArq_Imagem[20] = "/Images/wcab.png";
            aPronuncia[20] = "pcab.wav";
            aLetras[20, 0] = "c";
            aLetras[20, 1] = "a";
            aLetras[20, 2] = "b";
            aLetras[20, 3] = "";
            aLetras[20, 4] = "";
            aLetras[20, 5] = "";

            aPalavras[21] = "mirror";
            aPortugues[21] = "espelho";
            aDicas[21] = "objeto";
            aArq_Imagem[21] = "/Images/wmirror.png";
            aPronuncia[21] = "pmirror.wav";
            aLetras[21, 0] = "m";
            aLetras[21, 1] = "i";
            aLetras[21, 2] = "r";
            aLetras[21, 3] = "r";
            aLetras[21, 4] = "o";
            aLetras[21, 5] = "r";

            aPalavras[22] = "doctor";
            aPortugues[22] = "médico";
            aDicas[22] = "profissão";
            aArq_Imagem[22] = "/Images/wdoctor.png";
            aPronuncia[22] = "pdoctor.wav";
            aLetras[22, 0] = "d";
            aLetras[22, 1] = "o";
            aLetras[22, 2] = "c";
            aLetras[22, 3] = "t";
            aLetras[22, 4] = "o";
            aLetras[22, 5] = "r";

            aPalavras[23] = "cheese";
            aPortugues[23] = "queijo";
            aDicas[23] = "comida";
            aArq_Imagem[23] = "/Images/wcheese.png";
            aPronuncia[23] = "pcheese.wav";
            aLetras[23, 0] = "c";
            aLetras[23, 1] = "h";
            aLetras[23, 2] = "e";
            aLetras[23, 3] = "e";
            aLetras[23, 4] = "s";
            aLetras[23, 5] = "e";

            aPalavras[24] = "fish";
            aPortugues[24] = "peixe";
            aDicas[24] = "animal";
            aArq_Imagem[24] = "/Images/wfish.png";
            aPronuncia[24] = "pfish.wav";
            aLetras[24, 0] = "f";
            aLetras[24, 1] = "i";
            aLetras[24, 2] = "s";
            aLetras[24, 3] = "h";
            aLetras[24, 4] = "";
            aLetras[24, 5] = "";

            aPalavras[25] = "soccer";
            aPortugues[25] = "futebol";
            aDicas[25] = "esporte";
            aArq_Imagem[25] = "/Images/wsoccer.png";
            aPronuncia[25] = "psoccer.wav";
            aLetras[25, 0] = "s";
            aLetras[25, 1] = "o";
            aLetras[25, 2] = "c";
            aLetras[25, 3] = "c";
            aLetras[25, 4] = "e";
            aLetras[25, 5] = "r";

            aPalavras[26] = "red";
            aPortugues[26] = "vermelho";
            aDicas[26] = "cores";
            aArq_Imagem[26] = "/Images/wred.png";
            aPronuncia[26] = "pred.wav";
            aLetras[26, 0] = "r";
            aLetras[26, 1] = "e";
            aLetras[26, 2] = "d";
            aLetras[26, 3] = "";
            aLetras[26, 4] = "";
            aLetras[26, 5] = "";

            aPalavras[27] = "green";
            aPortugues[27] = "verde";
            aDicas[27] = "cores";
            aArq_Imagem[27] = "/Images/wgreen.png";
            aPronuncia[27] = "pgreen.wav";
            aLetras[27, 0] = "g";
            aLetras[27, 1] = "r";
            aLetras[27, 2] = "e";
            aLetras[27, 3] = "e";
            aLetras[27, 4] = "n";
            aLetras[27, 5] = "";

            aPalavras[28] = "yellow";
            aPortugues[28] = "amarelo";
            aDicas[28] = "cores";
            aArq_Imagem[28] = "/Images/wyellow.png";
            aPronuncia[28] = "pyellow.wav";
            aLetras[28, 0] = "y";
            aLetras[28, 1] = "e";
            aLetras[28, 2] = "l";
            aLetras[28, 3] = "l";
            aLetras[28, 4] = "o";
            aLetras[28, 5] = "w";

            aPalavras[29] = "brown";
            aPortugues[29] = "marrom";
            aDicas[29] = "cores";
            aArq_Imagem[29] = "/Images/wbrown.png";
            aPronuncia[29] = "pbrown.wav";
            aLetras[29, 0] = "b";
            aLetras[29, 1] = "r";
            aLetras[29, 2] = "o";
            aLetras[29, 3] = "w";
            aLetras[29, 4] = "n";
            aLetras[29, 5] = "";

        }

        public int sorteiaPalavra()
        {
            string imagemSelecionada;
            int sorteio;
            

            //zera variaveis publicas
            letra_escolhida = "";
            resultadoLetra0 = "";
            resultadoLetra1 = "";
            resultadoLetra2 = "";
            resultadoLetra3 = "";
            resultadoLetra4 = "";
            resultadoLetra5 = "";
            som_escolhido = "";
            img_escolhida = "";
            contaErros = 0;

            //zera imagens nos botões Word
            BitmapImage imgLimpa = new BitmapImage(new Uri("", UriKind.Relative));
            imgWord0.Source=imgLimpa;
            imgWord1.Source=imgLimpa;
            imgWord2.Source=imgLimpa;
            imgWord3.Source=imgLimpa;
            imgWord4.Source=imgLimpa;
            imgWord5.Source=imgLimpa;
            lblNroErros.Content = "";
            
            //realiza o sorteio da nova palavra a partir da base
            Random r = new Random();
            sorteio = r.Next(0, 30);
            while (nroSorteado == sorteio)
            {
                sorteio = r.Next(0, 30);
            }
            nroSorteado = sorteio;

            //carrega as informações da nova palavra a partir do numero sorteado.
            imagemSelecionada = aArq_Imagem[nroSorteado];
            BitmapImage img = new BitmapImage(new Uri(imagemSelecionada, UriKind.Relative));
            imgPalavra.Source = img;

            lblDicaPalavra.Content = aDicas[nroSorteado];
            lblPalavraPortugues.Content = aPortugues[nroSorteado];

            //if (string.IsNullOrEmpty(letra0))
            if ( string.IsNullOrEmpty(aLetras[nroSorteado,0]))
                btnWord0.IsEnabled = false;
            else
                btnWord0.IsEnabled = true;

            if (string.IsNullOrEmpty(aLetras[nroSorteado, 1]))
                btnWord1.IsEnabled = false;
            else
                btnWord1.IsEnabled = true;

            if (string.IsNullOrEmpty(aLetras[nroSorteado, 2]))
                btnWord2.IsEnabled = false;
            else
                btnWord2.IsEnabled = true;

            if (string.IsNullOrEmpty(aLetras[nroSorteado, 3]))
                btnWord3.IsEnabled = false;
            else
                btnWord3.IsEnabled = true;

            if (string.IsNullOrEmpty(aLetras[nroSorteado, 4]))
                btnWord4.IsEnabled = false;
            else
                btnWord4.IsEnabled = true;

            if (string.IsNullOrEmpty(aLetras[nroSorteado, 5]))
                btnWord5.IsEnabled = false;
            else
                btnWord5.IsEnabled = true;

            return 0;
        }

        public void verificaJogo()
        {
            if ( aLetras[nroSorteado,0].ToUpper()==resultadoLetra0 &&
                aLetras[nroSorteado,1].ToUpper()==resultadoLetra1 &&
                aLetras[nroSorteado,2].ToUpper()==resultadoLetra2 &&
                aLetras[nroSorteado,3].ToUpper()==resultadoLetra3 &&
                aLetras[nroSorteado,4].ToUpper()==resultadoLetra4 &&
                aLetras[nroSorteado,5].ToUpper()==resultadoLetra5 )
            {
                ativaTelaParabens();
            }
            else
            {
                System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(somErro);
                myPlayer.Play();
            }
        }

        #endregion

        #region "*---------------Botões Letras-----------------------*"
        private void btnLetraA_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons+"letra_a.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "A";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_a.png";

        }

        private void btnLetraB_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_b.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "B";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_b.png";
        }

        private void btnLetraC_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_c.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();
            
            letra_escolhida = "C";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_c.png";
        }

        private void btnLetraD_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_d.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "D";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_d.png";
        }

        private void btnLetraE_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_e.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "E";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_e.png";
        }

        private void btnLetraF_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_f.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "F";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_f.png";
        }

        private void btnLetraG_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_g.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "G";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_g.png";
        }

        private void btnLetraH_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_h.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "H";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_h.png";
        }

        private void btnLetraI_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_i.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "I";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_i.png";
        }

        private void btnLetraJ_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons+"letra_j.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "J";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_j.png";
        }

        private void btnLetraK_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons+"letra_k.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "K";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_k.png";
        }

        private void btnLetraL_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons+"letra_l.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "L";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_l.png";
        }

        private void btnLetraM_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_m.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "M";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_m.png";
        }

        private void btnLetraN_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_n.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "N";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_n.png";
        }

        private void btnLetraO_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_o.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "O";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_o.png";
        }

        private void btnLetraP_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_p.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "P";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_p.png";
        }

        private void btnLetraQ_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_q.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "Q";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_q.png";
        }

        private void btnLetraR_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_r.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "R";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_r.png";
        }

        private void btnLetraS_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_s.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "S";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_s.png";
        }

        private void btnLetraT_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_t.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "T";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_t.png";
        }

        private void btnLetraU_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_u.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "U";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_u.png";
        }

        private void btnLetraV_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_v.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "V";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_v.png";
        }

        private void btnLetraW_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_w.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "W";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_w.png";
        }

        private void btnLetraX_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_x.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "X";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_x.png";
        }

        private void btnLetraY_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_y.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "Y";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_y.png";
        }

        private void btnLetraZ_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + "letra_z.wav";

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();

            letra_escolhida = "Z";
            som_escolhido = arquivo;
            img_escolhida = "/Images/letra_z.png";
        }
        #endregion

        #region "*---------------Botões Gerais----------------------*"
        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            sorteiaPalavra();
        }

        private void btnFigura_Click(object sender, RoutedEventArgs e)
        {
            String arquivo = dirSons + aPronuncia[nroSorteado].ToString();

            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(arquivo);
            myPlayer.Play();
        }

        private void btnSair_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ativaTelaParabens()
        {
            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(somInicio);
            myPlayer.PlayLooping();

            Forca.Margin = new Thickness(2000, 0, 0, 0);
            Parabens.Margin = new Thickness(10, 0, 0, 0);
        }

        private void ativaTelaJogo()
        {
            System.Media.SoundPlayer myPlayer = new System.Media.SoundPlayer(somAcerto);
            myPlayer.Play();
            Forca.Margin = new Thickness(10, 0, 0, 0);
            Parabens.Margin = new Thickness(2000, 0, 0, 0);
        }

        private void btnJogarNovamente_Click(object sender, RoutedEventArgs e)
        {
            sorteiaPalavra();
            ativaTelaJogo();
        }
        private void btnVerifica_Click(object sender, RoutedEventArgs e)
        {
            verificaJogo();
        }
        #endregion

        #region "*---------------Botão Palavra----------------------*"
        private void btnWord0_Click(object sender, RoutedEventArgs e)
        {
            System.Media.SoundPlayer myPlayer;
            if (btnWord0.IsEnabled == true)
            {
                if (aLetras[nroSorteado,0].ToUpper()==letra_escolhida)
                {
                    BitmapImage img = new BitmapImage(new Uri(img_escolhida, UriKind.Relative));
                    imgWord0.Source = img;

                    myPlayer = new System.Media.SoundPlayer(som_escolhido);
                    myPlayer.Play();

                    resultadoLetra0 = letra_escolhida;
                }
                else
                {
                    myPlayer = new System.Media.SoundPlayer(somErro);
                    myPlayer.Play();
                    contaErros++;
                    if (String.IsNullOrEmpty(resultadoLetra0))
                        resultadoLetra0 = "";
                    lblNroErros.Content = contaErros.ToString();
                }
            }
        }

        private void btnWord1_Click(object sender, RoutedEventArgs e)
        {
            System.Media.SoundPlayer myPlayer;
            if (btnWord1.IsEnabled == true)
            {
                if (aLetras[nroSorteado, 1].ToUpper() == letra_escolhida)
                {
                    BitmapImage img = new BitmapImage(new Uri(img_escolhida, UriKind.Relative));
                    imgWord1.Source = img;

                    myPlayer = new System.Media.SoundPlayer(som_escolhido);
                    myPlayer.Play();

                    resultadoLetra1 = letra_escolhida;
                }
                else
                {
                    myPlayer = new System.Media.SoundPlayer(somErro);
                    myPlayer.Play();
                    contaErros++;
                    if (String.IsNullOrEmpty(resultadoLetra1))
                        resultadoLetra1 = "";
                    lblNroErros.Content = contaErros.ToString();
                }
            }
        }

        private void btnWord2_Click(object sender, RoutedEventArgs e)
        {
            System.Media.SoundPlayer myPlayer;
            if (btnWord2.IsEnabled == true)
            {
                if (aLetras[nroSorteado, 2].ToUpper() == letra_escolhida)
                {
                    BitmapImage img = new BitmapImage(new Uri(img_escolhida, UriKind.Relative));
                    imgWord2.Source = img;

                    myPlayer = new System.Media.SoundPlayer(som_escolhido);
                    myPlayer.Play();

                    resultadoLetra2 = letra_escolhida;

                }
                else
                {
                    myPlayer = new System.Media.SoundPlayer(somErro);
                    myPlayer.Play();
                    contaErros++;
                    if (String.IsNullOrEmpty(resultadoLetra2))
                        resultadoLetra2 = "";
                    lblNroErros.Content = contaErros.ToString();
                }
            }
        }

        private void btnWord3_Click(object sender, RoutedEventArgs e)
        {
            System.Media.SoundPlayer myPlayer;
            if (btnWord3.IsEnabled == true)
            {
                if (aLetras[nroSorteado, 3].ToUpper() == letra_escolhida)
                {
                    BitmapImage img = new BitmapImage(new Uri(img_escolhida, UriKind.Relative));
                    imgWord3.Source = img;

                    myPlayer = new System.Media.SoundPlayer(som_escolhido);
                    myPlayer.Play();

                    resultadoLetra3 = letra_escolhida;

                }
                else
                {
                    myPlayer = new System.Media.SoundPlayer(somErro);
                    myPlayer.Play();
                    contaErros++;
                    if (String.IsNullOrEmpty(resultadoLetra3))
                        resultadoLetra3 = "";
                    lblNroErros.Content = contaErros.ToString();
                }
            }
        }

        private void btnWord4_Click(object sender, RoutedEventArgs e)
        {
            System.Media.SoundPlayer myPlayer;
            if (btnWord4.IsEnabled == true)
            {
                if (aLetras[nroSorteado, 4].ToUpper() == letra_escolhida)
                {
                    BitmapImage img = new BitmapImage(new Uri(img_escolhida, UriKind.Relative));
                    imgWord4.Source = img;

                    myPlayer = new System.Media.SoundPlayer(som_escolhido);
                    myPlayer.Play();

                    resultadoLetra4 = letra_escolhida;

                }
                else
                {
                    myPlayer = new System.Media.SoundPlayer(somErro);
                    myPlayer.Play();
                    contaErros++;
                    if (String.IsNullOrEmpty(resultadoLetra4))
                        resultadoLetra4 = "";
                    lblNroErros.Content = contaErros.ToString();
                }
            }
        }

        private void btnWord5_Click(object sender, RoutedEventArgs e)
        {
            System.Media.SoundPlayer myPlayer;
            if (btnWord5.IsEnabled == true)
            {
                if (aLetras[nroSorteado, 5].ToUpper() == letra_escolhida)
                {
                    BitmapImage img = new BitmapImage(new Uri(img_escolhida, UriKind.Relative));
                    imgWord5.Source = img;

                    myPlayer = new System.Media.SoundPlayer(som_escolhido);
                    myPlayer.Play();

                    resultadoLetra5 = letra_escolhida;
                }
                else
                {
                    myPlayer = new System.Media.SoundPlayer(somErro);
                    myPlayer.Play();
                    contaErros++;
                    if (String.IsNullOrEmpty(resultadoLetra5))
                        resultadoLetra5 = "";
                    lblNroErros.Content = contaErros.ToString();
                }
            }
        }
        #endregion



    }
}
