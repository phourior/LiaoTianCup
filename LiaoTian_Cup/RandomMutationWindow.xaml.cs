﻿using Force.DeepCloner;
using LiaoTian_Cup.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace LiaoTian_Cup
{
    /// <summary>
    /// RandomMutationWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RandomMutationWindow : Window, INotifyPropertyChanged
    {
        //路径
        private readonly string mutationFilePath = "./Resources/自选突变列表.csv";
        private readonly string mutationFactorPath = "./Resources/突变因子列表.csv";
        private readonly string beforeCommanderFilePath = "./Resources/先出指挥官列表.csv";
        private readonly string afterCommanderFilePath = "./Resources/后出指挥官列表.csv";
        private readonly string aIFilePath = "./Resources/电脑AI.csv";


        //存放从自选突变CSV中得到的数据
        private List<string[]> mutationList = new List<string[]>();

        //存放从突变因子CSV中得到的数据
        private List<string> mutationFactorList = new List<string>();

        //存放先出指挥官CSV中得到的数据
        private List<string> beforeCommanderInfo = new List<string>();
        //存放后出指挥官CSV中得到的数据
        private List<string> afterCommanderInfo = new List<string>();

        //存放所有的人机CSV中得到的数据
        private List<string> botInfo = new List<string>();
        public string botName = "暂未随机AI";

        //链表，存放自选因子
        private List<Image> hasSelectFactor = new List<Image>(8);
        private Image hasSelectCommander = new Image();

        //初始化
        RandomKit rk = new RandomKit();
        public event PropertyChangedEventHandler PropertyChanged;

        //是否允许随机AI逻辑
        private bool _isRandAI;
        public bool isRandAI {
            get { return _isRandAI; }
            set
            {
                _isRandAI = value;
                RaisePropertyChanged(nameof(isRandAI));
            }
        }


        //玩家名响应相关逻辑
        private string _playName;
        public string playName
        {
            get { return _playName; }
            set
            {
               _playName = value;
                RaisePropertyChanged(nameof(playName));
            }
        }
        

        public RandomMutationWindow()
        {
            //初始化窗口时即拿数据
            CSVKit.Csv2Dt(mutationFilePath, mutationList);
            CSVKit.Csv2Dt(mutationFactorPath, mutationFactorList);
            CSVKit.Csv2Dt(beforeCommanderFilePath, beforeCommanderInfo);
            CSVKit.Csv2Dt(afterCommanderFilePath, afterCommanderInfo);
            CSVKit.Csv2Dt(aIFilePath, botInfo);
            InitializeComponent();
            this.DataContext = this;
        }

        //随机得到一条官突数据stirng[],8因子和指挥官也会在此进行随机
        private void RandomMutationFunc()
        {
            //TODO 有重复出现同一个突变的情况
            Random rand = new Random();
            int number = rand.Next(0, mutationList.Count);

            string[] randMutationInfo = mutationList[number];
            MutationBox.Text = randMutationInfo[0];//突变名称
            MapBox.Text = randMutationInfo[1];//地图名称

            //相对路径URI指定随机突变地图和因子图片来源
            MapImg.Source = new BitmapImage(new Uri("./Resources/maps/" + randMutationInfo[1] + ".png", UriKind.Relative));
            Factor1.Source = new BitmapImage(new Uri("./Resources/factor/" + randMutationInfo[2] + ".png", UriKind.Relative));
            Factor2.Source = new BitmapImage(new Uri("./Resources/factor/" + randMutationInfo[3] + ".png", UriKind.Relative));
            Factor3.Source = new BitmapImage(new Uri("./Resources/factor/" + randMutationInfo[4] + ".png", UriKind.Relative));
            //随机8因子选择
            Random8Factor(randMutationInfo);
            //随机先出和后出指挥官
            RandomCommanderInfo();
        }

        //随机8因子选择处理逻辑
        private void Random8Factor(string[] randMutationInfo)
        {
            var factorListClone = mutationFactorList.DeepClone();
            for (int i = 2; i < randMutationInfo.Length; i++)
            {
                factorListClone.Remove(randMutationInfo[i]);
            }
            List<int> rand8Num = rk.GenerateXRandomNum(8, factorListClone.Count);

            //相对路径URI显示8个因子的图片
            SelectFactor1.Source = new BitmapImage(new Uri("./Resources/factor/" + factorListClone[rand8Num[0]] + ".png", UriKind.Relative));
            SelectFactor2.Source = new BitmapImage(new Uri("./Resources/factor/" + factorListClone[rand8Num[1]] + ".png", UriKind.Relative));
            SelectFactor3.Source = new BitmapImage(new Uri("./Resources/factor/" + factorListClone[rand8Num[2]] + ".png", UriKind.Relative));
            SelectFactor4.Source = new BitmapImage(new Uri("./Resources/factor/" + factorListClone[rand8Num[3]] + ".png", UriKind.Relative));
            SelectFactor5.Source = new BitmapImage(new Uri("./Resources/factor/" + factorListClone[rand8Num[4]] + ".png", UriKind.Relative));
            SelectFactor6.Source = new BitmapImage(new Uri("./Resources/factor/" + factorListClone[rand8Num[5]] + ".png", UriKind.Relative));
            SelectFactor7.Source = new BitmapImage(new Uri("./Resources/factor/" + factorListClone[rand8Num[6]] + ".png", UriKind.Relative));
            SelectFactor8.Source = new BitmapImage(new Uri("./Resources/factor/" + factorListClone[rand8Num[7]] + ".png", UriKind.Relative));
        }

        //随机先出和后出指挥官处理逻辑
        private void RandomCommanderInfo()
        {
            List<int> beforeRandNum = rk.GenerateXRandomNum(4, beforeCommanderInfo.Count);
            List<int> afterRandNum = rk.GenerateXRandomNum(2, afterCommanderInfo.Count);

            //相对路径URI指定指挥官图片来源
            BeforeCommander1.Source = new BitmapImage(new Uri("./Resources/commander/" + beforeCommanderInfo[beforeRandNum[0]] + ".png", UriKind.Relative));
            BeforeCommander2.Source = new BitmapImage(new Uri("./Resources/commander/" + beforeCommanderInfo[beforeRandNum[1]] + ".png", UriKind.Relative));
            BeforeCommander3.Source = new BitmapImage(new Uri("./Resources/commander/" + beforeCommanderInfo[beforeRandNum[2]] + ".png", UriKind.Relative));
            BeforeCommander4.Source = new BitmapImage(new Uri("./Resources/commander/" + beforeCommanderInfo[beforeRandNum[3]] + ".png", UriKind.Relative));

            AfterCommander1.Source = new BitmapImage(new Uri("./Resources/commander/" + afterCommanderInfo[afterRandNum[0]] + ".png", UriKind.Relative));
            AfterCommander2.Source = new BitmapImage(new Uri("./Resources/commander/" + afterCommanderInfo[afterRandNum[1]] + ".png", UriKind.Relative));
        }

        
        
        //是否随机AI的处理逻辑
        private string IsRandAIFunc()
        {
            if (isRandAI)
            {
                Random rand = new Random();
                int number = rand.Next(0, botInfo.Count);
                return botInfo[number];
            }
            else
            {
                return "暂未随机AI";
            }
        }

        //返回主页事件响应
        private void Button_BackMain_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //开始随机事件响应
        private void Button_Random_Click(object sender, RoutedEventArgs e)
        {
            reflashSelectItem();
            RandomMutationFunc();
        }

        //点击自选因子事件响应
        private void Factor_MouseDown(object sender, RoutedEventArgs e)
        {
            Image selectFactor = (Image)sender;
            if (selectFactor != null)
            {
                if (!hasSelectFactor.Contains(selectFactor))
                {
                    hasSelectFactor.Add(selectFactor);
                }
                else
                {
                    return;
                }
            }
            FlashHasSelectFactor();
        }

        //点击已选择的自选因子 取消事件响应
        private void CancelFactor_MouseDown(object sender, RoutedEventArgs e)
        {
            Image cancelFactor = (Image)sender;
            if (cancelFactor != null)
            {
                for (int i = 0; i < hasSelectFactor.Count; i++)
                {
                    if (hasSelectFactor[i] != null
                        && hasSelectFactor[i].Source.ToString().Equals(cancelFactor.Source.ToString()))
                    {
                        hasSelectFactor.RemoveAt(i);
                    }
                }
            }
            else { return; }
            FlashHasSelectFactor();
        }

        //点击自选指挥官事件响应
        private void Commander_MouseDown(object sender, RoutedEventArgs e)
        {
            CommanderWarn.Text = "";
            Image selectCommander = (Image)sender;
            if (selectCommander != null)
            {
                hasSelectCommander = selectCommander;
            }
            FlashHasSelectCommander();
        }

        //取消当前选择的指挥官事件响应
        private void CancelCommander_MouseDown(Object sender, RoutedEventArgs e)
        {
            Image cancelCommander = (Image)sender;
            if (cancelCommander != null)
            {
                hasSelectCommander = new Image();
            }
            else { return; }
            FlashHasSelectCommander();
        }

        //确认按钮事件响应
        private void Button_Confirm_Click(object sender, RoutedEventArgs e)
        {
            botName = IsRandAIFunc();
            if(hasSelectCommander == null || hasSelectCommander.Source == null || hasSelectCommander.Source.Equals(""))
            {
                CommanderWarn.Text = "未选择指挥官";
                return;
            }
            else
            {
                CommanderWarn.Text = "";
            }
            this.Hide();
            ShowRMDetail showRMDetail = new ShowRMDetail(this);
            showRMDetail.Show();
        }

        //刷新已选择的因子
        private void FlashHasSelectFactor()
        {
            HasSelectFactor1.Source = hasSelectFactor.Count < 1 ? null : hasSelectFactor[0].Source;
            HasSelectFactor2.Source = hasSelectFactor.Count < 2 ? null : hasSelectFactor[1].Source;
            HasSelectFactor3.Source = hasSelectFactor.Count < 3 ? null : hasSelectFactor[2].Source;
            HasSelectFactor4.Source = hasSelectFactor.Count < 4 ? null : hasSelectFactor[3].Source;
            HasSelectFactor5.Source = hasSelectFactor.Count < 5 ? null : hasSelectFactor[4].Source;
            HasSelectFactor6.Source = hasSelectFactor.Count < 6 ? null : hasSelectFactor[5].Source;
            HasSelectFactor7.Source = hasSelectFactor.Count < 7 ? null : hasSelectFactor[6].Source;
            HasSelectFactor8.Source = hasSelectFactor.Count < 8 ? null : hasSelectFactor[7].Source;
        }

        //刷新选择的指挥官
        private void FlashHasSelectCommander()
        {
            if (hasSelectCommander != null)
            {
                HasSelectCommander.Source = hasSelectCommander.Source;
            }
        }

        //重写关闭窗口事件
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Application.Current.MainWindow.Show();
        }

        //刷新窗口的方法
        internal void reflashSelectItem()
        {
            hasSelectFactor.Clear();
            hasSelectCommander = new Image();
            //刷新
            FlashHasSelectFactor();
            FlashHasSelectCommander();
        }

        //实现响应接口
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}