﻿using Force.DeepCloner;
using main.Helper;
using main.Overview;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace main.Mode
{
    /// <summary>
    /// HubModeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HubModeWindow : Page
    {
        public string botName = Dictionary.I18n.Lang.ResourceManager.GetString("RandomBotStrTip");

        //链表，存放自选因子
        private List<Image> hasSelectBase = new List<Image>(3);
        private List<Image> hasSelectFixedFactor = new List<Image>(2);
        private List<Image> hasSelectFactor = new List<Image>();
        private List<Image> hasSelectCommander = new List<Image>(2);
        private Image hasSelectMap = new Image();


        //初始化工具
        readonly RandomKit rk = new RandomKit();
        string factorDir = Dictionary.FilePath.factorDir;
        string mapDir = Dictionary.FilePath.mapDir;
        string commanderDir = Dictionary.FilePath.commanderDir;
        public event PropertyChangedEventHandler PropertyChanged;

        public HubModeWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        //是否允许随机AI逻辑
        private bool _isRandAI;
        public bool isRandAI
        {
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

        //模式选择（6因子模式,8因子模式）数据
        private string _modeName = Dictionary.I18n.Lang.ResourceManager.GetString("SixMutatorsMode");
        public string modeName
        {
            get { return _modeName; }
            set
            {
                _modeName = value;
                RaisePropertyChanged(nameof(_modeName));
            }
        }

        //是否随机AI的处理逻辑
        private string IsRandAIFunc()
        {
            if (isRandAI)
            {
                Random rand = new Random();
                int number = rand.Next(0, FileData.botInfo.Count);
                return Dictionary.I18n.Lang.ResourceManager.GetString(FileData.botInfo[number]);
            }
            else
            {
                return Dictionary.I18n.Lang.ResourceManager.GetString("RandomBotStrTip");
            }
        }

        private void Button_Reset_Click(object sender, RoutedEventArgs e)
        {
            ResetFunc();
        }

        public void ResetFunc()
        {
            Warn.Text = "";
            ModeBox.IsEnabled = true;
            RandStartBtn.IsEnabled = true;
            SetRandMapEnable(true);
            SetBaseFactorEnable(true);

            hasSelectBase.Clear();
            BaseConfirmBtn.IsEnabled = false;
            ClearBaseNegativeFactor();
            ClearBaseMultiFactor();
            FlashSelectBase();

            hasSelectFixedFactor.Clear();
            ClearFixedFactor();

            hasSelectFactor.Clear();
            ClearRandomFactor();
            FlashHasSelectFactor();

            hasSelectCommander.Clear();
            clearRandomCommander();
            FlashHasSelectCommander();

            hasSelectMap = new Image();
            ClearRandomMaps();
            FlashHasSelectMap();

            MapLabel.Foreground = Brushes.Black;
            NegativeLabel.Foreground = Brushes.Black;
            MultilineLabel.Foreground = Brushes.Black;
            FreeLabel.Foreground = Brushes.Black;
            OldCmdrLabel.Foreground = Brushes.Black;
            NewCmdrLabel.Foreground = Brushes.Black;
        }

        //开始随机事件响应
        private void Button_Random_Click(object sender, RoutedEventArgs e)
        {
            ModeBox.IsEnabled = false;
            RandStartBtn.IsEnabled = false;
            ShowRandomMaps();
        }

        //随机地图显示清除
        private void ClearRandomMaps()
        {
            MapImg1.Source = new BitmapImage();
            MapImg2.Source = new BitmapImage();
            MapImg3.Source = new BitmapImage();
            MapName1.Text = "";
            MapName2.Text = "";
            MapName3.Text = "";
        }

        //返回主页事件响应
        private void Button_BackMain_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        //随机地图显示
        private void ShowRandomMaps()
        {
            hasSelectMap = new Image();
            FlashHasSelectMap();

            if (_modeName.Equals(Dictionary.I18n.Lang.ResourceManager.GetString("BraveMode")))
            {
                List<int> randNums = rk.GenerateXRandomNum(3, FileData.braveMapsInfo.Count);
                MapImg1.Source = new BitmapImage(new Uri(mapDir + FileData.braveMapsInfo[randNums[0]] + ".png", UriKind.Relative));
                MapImg2.Source = new BitmapImage(new Uri(mapDir + FileData.braveMapsInfo[randNums[1]] + ".png", UriKind.Relative));
                MapImg3.Source = new BitmapImage(new Uri(mapDir + FileData.braveMapsInfo[randNums[2]] + ".png", UriKind.Relative));

                MapName1.Text = Dictionary.I18n.Lang.ResourceManager.GetString(FileData.braveMapsInfo[randNums[0]]);
                MapName2.Text = Dictionary.I18n.Lang.ResourceManager.GetString(FileData.braveMapsInfo[randNums[1]]);
                MapName3.Text = Dictionary.I18n.Lang.ResourceManager.GetString(FileData.braveMapsInfo[randNums[2]]);
            }
            else
            {
                List<int> randNums = rk.GenerateXRandomNum(3, FileData.hubMapsInfo.Count);
                MapImg1.Source = new BitmapImage(new Uri(mapDir + FileData.hubMapsInfo[randNums[0]] + ".png", UriKind.Relative));
                MapImg2.Source = new BitmapImage(new Uri(mapDir + FileData.hubMapsInfo[randNums[1]] + ".png", UriKind.Relative));
                MapImg3.Source = new BitmapImage(new Uri(mapDir + FileData.hubMapsInfo[randNums[2]] + ".png", UriKind.Relative));

                MapName1.Text = Dictionary.I18n.Lang.ResourceManager.GetString(FileData.hubMapsInfo[randNums[0]]);
                MapName2.Text = Dictionary.I18n.Lang.ResourceManager.GetString(FileData.hubMapsInfo[randNums[1]]);
                MapName3.Text = Dictionary.I18n.Lang.ResourceManager.GetString(FileData.hubMapsInfo[randNums[2]]);
            }

            MapLabel.Foreground = Brushes.Red;
        }

        //点击地图图片事件响应
        private void Maps_MouseDown(object sender, MouseEventArgs e)
        {
            Warn.Text = "";
            Image selectMap = (Image)sender;
            if (selectMap != null)
            {
                hasSelectMap = selectMap;
            }
            FlashHasSelectMap();
        }

        //取消当前选择的地图事件响应(Unused)
        private void CancelMap_MouseDown(object sender, RoutedEventArgs e)
        {
            Image cancelMap = (Image)sender;
            if (cancelMap != null)
            {
                hasSelectMap = new Image();
            }
            else { return; }
            FlashHasSelectMap();
        }

        //刷新选择的地图
        private void FlashHasSelectMap()
        {
            HasSelectMap.Source = hasSelectMap.Source;
            MapTip.Text = "";
            if (hasSelectMap.Source != null)
            {
                var currentMapName = (hasSelectMap.Source as BitmapImage).UriSource.ToString()
                        .Replace("/main;component/Resources/maps/", "").Replace(".png", "");
                if (!string.IsNullOrEmpty(currentMapName))
                {
                    MapTip.Text = Dictionary.I18n.Lang.ResourceManager.GetString(currentMapName);
                }
                else
                {
                    MapTip.Text = "";
                }
            }
        }

        //确认地图按钮事件响应
        private void Button_MapConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (hasSelectMap == null || hasSelectMap.Source == null || hasSelectMap.Source.Equals(""))
            {
                Warn.Text = Dictionary.I18n.Lang.ResourceManager.GetString("MapWarn");
                return;
            }
            MapLabel.Foreground = Brushes.Gray;
            SetRandMapEnable(false);
            ShowFixedFactor();
            ShowBaseNegativeFactor();
            ShowBaseMultiFactor();
        }

        //固定因子显示
        private void ShowFixedFactor()
        {
            //相对路径URI指定因子图片来源
            List<int> randNum = rk.GenerateXRandomNum(2, FileData.hubFixedFactorInfo.Count);
            string uri = factorDir + FileData.hubFixedFactorInfo[randNum[0]] + ".png";
            FixedFactor1.Source = new BitmapImage(new Uri(uri, UriKind.Relative));

            if (_modeName.Equals(Dictionary.I18n.Lang.ResourceManager.GetString("EightMutatorsMode")))
            {
                FixedFactor2.Source = new BitmapImage(new Uri((factorDir + FileData.hubFixedFactorInfo[randNum[1]] + ".png"), UriKind.Relative));
            }

        }

        //正面因子显示
        private void ShowBaseNegativeFactor()
        {
            NegativeLabel.Foreground = Brushes.Red;
            //相对路径URI指定因子图片来源
            NegativeFactor1.Source = new BitmapImage(new Uri(factorDir + FileData.hubNegativeFactorInfo[0] + ".png", UriKind.Relative));
            NegativeFactor2.Source = new BitmapImage(new Uri(factorDir + FileData.hubNegativeFactorInfo[1] + ".png", UriKind.Relative));
            NegativeFactor3.Source = new BitmapImage(new Uri(factorDir + FileData.hubNegativeFactorInfo[2] + ".png", UriKind.Relative));
            NegativeFactor4.Source = new BitmapImage(new Uri(factorDir + FileData.hubNegativeFactorInfo[3] + ".png", UriKind.Relative));
            NegativeFactor5.Source = new BitmapImage(new Uri(factorDir + FileData.hubNegativeFactorInfo[4] + ".png", UriKind.Relative));
            NegativeFactor6.Source = new BitmapImage(new Uri(factorDir + FileData.hubNegativeFactorInfo[5] + ".png", UriKind.Relative));
            NegativeFactor7.Source = new BitmapImage(new Uri(factorDir + FileData.hubNegativeFactorInfo[6] + ".png", UriKind.Relative));
            NegativeFactor8.Source = new BitmapImage(new Uri(factorDir + FileData.hubNegativeFactorInfo[7] + ".png", UriKind.Relative));
            NegativeFactor9.Source = new BitmapImage(new Uri(factorDir + FileData.hubNegativeFactorInfo[8] + ".png", UriKind.Relative));
            NegativeFactor10.Source = new BitmapImage(new Uri(factorDir + FileData.hubNegativeFactorInfo[9] + ".png", UriKind.Relative));
            NegativeFactor11.Source = new BitmapImage(new Uri(factorDir + FileData.hubNegativeFactorInfo[10] + ".png", UriKind.Relative));
            NegativeFactor12.Source = new BitmapImage(new Uri(factorDir + FileData.hubNegativeFactorInfo[11] + ".png", UriKind.Relative));

        }

        //多线因子显示
        private void ShowBaseMultiFactor()
        {
            MultilineLabel.Foreground = Brushes.Red;
            MultiFactor1.Source = new BitmapImage(new Uri(factorDir + FileData.hubMultiFactorInfo[0] + ".png", UriKind.Relative));
            MultiFactor2.Source = new BitmapImage(new Uri(factorDir + FileData.hubMultiFactorInfo[1] + ".png", UriKind.Relative));
            MultiFactor3.Source = new BitmapImage(new Uri(factorDir + FileData.hubMultiFactorInfo[2] + ".png", UriKind.Relative));
            MultiFactor4.Source = new BitmapImage(new Uri(factorDir + FileData.hubMultiFactorInfo[3] + ".png", UriKind.Relative));
            MultiFactor5.Source = new BitmapImage(new Uri(factorDir + FileData.hubMultiFactorInfo[4] + ".png", UriKind.Relative));

        }

        //点击基础因子(正面和多面)图片事件响应
        private void Base_MouseDown(object sender, MouseEventArgs e)
        {
            Warn.Text = "";
            Image selectBase = (Image)sender;
            if (selectBase != null)
            {
                if ((FixedFactor1.Source as BitmapImage).UriSource.ToString().Equals((selectBase.Source as BitmapImage).UriSource.ToString()))
                {
                    return;
                }

                if (FixedFactor2.Source != null)
                {
                    if ((FixedFactor2.Source as BitmapImage).UriSource.ToString().Equals((selectBase.Source as BitmapImage).UriSource.ToString()))
                    {
                        return;
                    }
                }

                if ((_modeName.Equals(Dictionary.I18n.Lang.ResourceManager.GetString("SixMutatorsMode"))
                    || _modeName.Equals(Dictionary.I18n.Lang.ResourceManager.GetString("BraveMode")))
                    && hasSelectBase != null 
                    && !hasSelectBase.Contains(selectBase))
                {
                    if (hasSelectBase.Count < 2)
                    {
                        hasSelectBase.Add(selectBase);
                    }
                }

                if (_modeName.Equals(Dictionary.I18n.Lang.ResourceManager.GetString("EightMutatorsMode")) && hasSelectBase != null && !hasSelectBase.Contains(selectBase))
                {
                    if (hasSelectBase.Count < 1)
                    {
                        hasSelectBase.Add(selectBase);
                    }
                }
            }
            FlashSelectBase();
        }

        //取消当前选择的基础因子事件响应
        private void CancelBase_MouseDown(Object sender, RoutedEventArgs e)
        {
            Image cancelBase = (Image)sender;
            if (cancelBase != null)
            {
                for (int i = 0; i < hasSelectBase.Count; i++)
                {
                    if (hasSelectBase[i] != null
                        && hasSelectBase[i].Source.ToString().Equals(cancelBase.Source.ToString()))
                    {
                        hasSelectBase.RemoveAt(i);
                    }
                }
            }
            else { return; }
            FlashSelectBase();
        }

        //确认基础因子按钮逻辑响应
        private void Button_BaseConfirm_Click(Object sender, RoutedEventArgs e)
        {
            Warn.Text = "";
            if (_modeName.Equals(Dictionary.I18n.Lang.ResourceManager.GetString("SixMutatorsMode")) && hasSelectBase.Count < 2)
            {
                Warn.Text = Dictionary.I18n.Lang.ResourceManager.GetString("BaseMutatorsWarn");
                return;
            }
            if (_modeName.Equals(Dictionary.I18n.Lang.ResourceManager.GetString("EightMutatorsMode")) && hasSelectBase.Count < 1)
            {
                Warn.Text = Dictionary.I18n.Lang.ResourceManager.GetString("BaseMutatorsWarn");
                return;
            }
            NegativeLabel.Foreground = Brushes.Gray;
            MultilineLabel.Foreground = Brushes.Gray;

            SetBaseFactorEnable(false);
            ShowRandomFactor();
            RandomCommanderInfo();
            OldCmdrLabel.Foreground = Brushes.Red;
            NewCmdrLabel.Foreground = Brushes.Red;
            FreeLabel.Foreground = Brushes.Red;
        }

        //刷新已选基础因子事件
        private void FlashSelectBase()
        {
            if (hasSelectBase != null)
            {
                HasSelectBaseFactor1.Source = hasSelectBase.Count < 1 ? null : hasSelectBase[0].Source;
                HasSelectBaseFactor2.Source = hasSelectBase.Count < 2 ? null : hasSelectBase[1].Source;
                HasSelectBaseFactor3.Source = hasSelectBase.Count < 3 ? null : hasSelectBase[2].Source;
            }
        }


        //显示最多8个随机选择因子
        private void ShowRandomFactor()
        {
            var factorListClone = FileData.hubFactorList.DeepClone();

            var currentFixedFactor1 = (FixedFactor1.Source as BitmapImage).UriSource.ToString()
                        .Replace("/main;component/Resources/factor/", "").Replace(".png", "");
            factorListClone.Remove(currentFixedFactor1);

            if (_modeName.Equals(Dictionary.I18n.Lang.ResourceManager.GetString("EightMutatorsMode")))
            {
                var currentFixedFactor2 = (FixedFactor2.Source as BitmapImage).UriSource.ToString()
                        .Replace("/main;component/Resources/factor/", "").Replace(".png", "");
                factorListClone.Remove(currentFixedFactor2);
            }

            for (int i = 0; i < hasSelectBase.Count; i++)
            {
                // 去除在前面已经选择过的因子
                var currentNegativeFactor = (hasSelectBase[i].Source as BitmapImage).UriSource.ToString()
                        .Replace("/main;component/Resources/factor/", "").Replace(".png", "");
                factorListClone.Remove(currentNegativeFactor);
            }

            List<int> randNum = rk.GenerateXRandomNum(8, factorListClone.Count);

            //相对路径URI显示6-8个因子的图片
            SelectFactor1.Source = new BitmapImage(new Uri(factorDir + factorListClone[randNum[0]] + ".png", UriKind.Relative));
            SelectFactor2.Source = new BitmapImage(new Uri(factorDir + factorListClone[randNum[1]] + ".png", UriKind.Relative));
            SelectFactor3.Source = new BitmapImage(new Uri(factorDir + factorListClone[randNum[2]] + ".png", UriKind.Relative));
            SelectFactor4.Source = new BitmapImage(new Uri(factorDir + factorListClone[randNum[3]] + ".png", UriKind.Relative));
            SelectFactor5.Source = new BitmapImage(new Uri(factorDir + factorListClone[randNum[4]] + ".png", UriKind.Relative));
            SelectFactor6.Source = new BitmapImage(new Uri(factorDir + factorListClone[randNum[5]] + ".png", UriKind.Relative));

            if (_modeName.Equals(Dictionary.I18n.Lang.ResourceManager.GetString("EightMutatorsMode")))
            {
                SelectFactor7.Source = new BitmapImage(new Uri(factorDir + factorListClone[randNum[6]] + ".png", UriKind.Relative));
            }
        }

        //点击自选因子事件响应
        private void Factor_MouseDown(object sender, RoutedEventArgs e)
        {
            Warn.Text = "";
            Image selectFactor = (Image)sender;
            if (selectFactor != null)
            {
                if ((_modeName.Equals(Dictionary.I18n.Lang.ResourceManager.GetString("SixMutatorsMode"))
                    || _modeName.Equals(Dictionary.I18n.Lang.ResourceManager.GetString("BraveMode")))
                    && hasSelectFactor != null && !hasSelectFactor.Contains(selectFactor))
                {
                    if (hasSelectFactor.Count < 3)
                    {
                        hasSelectFactor.Add(selectFactor);
                    }

                }

                if (_modeName.Equals(Dictionary.I18n.Lang.ResourceManager.GetString("EightMutatorsMode")) && hasSelectFactor != null && !hasSelectFactor.Contains(selectFactor))
                {
                    if (hasSelectFactor.Count < 5)
                    {
                        hasSelectFactor.Add(selectFactor);
                    }

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

        //刷新已选择的因子
        private void FlashHasSelectFactor()
        {
            if (hasSelectFactor != null)
            {
                HasSelectFactor1.Source = hasSelectFactor.Count < 1 ? null : hasSelectFactor[0].Source;
                HasSelectFactor2.Source = hasSelectFactor.Count < 2 ? null : hasSelectFactor[1].Source;
                HasSelectFactor3.Source = hasSelectFactor.Count < 3 ? null : hasSelectFactor[2].Source;
                HasSelectFactor4.Source = hasSelectFactor.Count < 4 ? null : hasSelectFactor[3].Source;

                if (_modeName.Equals(Dictionary.I18n.Lang.ResourceManager.GetString("EightMutatorsMode")))
                {
                    HasSelectFactor4.Source = hasSelectFactor.Count < 4 ? null : hasSelectFactor[3].Source;
                    HasSelectFactor5.Source = hasSelectFactor.Count < 5 ? null : hasSelectFactor[4].Source;
                }
            }
        }



        //随机先出和后出指挥官处理逻辑
        private void RandomCommanderInfo()
        {
            List<int> beforeRandNum = rk.GenerateXRandomNum(6, FileData.hubBeforeCommanderInfo.Count);
            List<int> afterRandNum = rk.GenerateXRandomNum(4, FileData.hubAfterCommanderInfo.Count);

            //相对路径URI指定指挥官图片来源
            BeforeCommander1.Source = new BitmapImage(new Uri(commanderDir + FileData.hubBeforeCommanderInfo[beforeRandNum[0]] + ".png", UriKind.Relative));
            BeforeCommander2.Source = new BitmapImage(new Uri(commanderDir + FileData.hubBeforeCommanderInfo[beforeRandNum[1]] + ".png", UriKind.Relative));
            BeforeCommander3.Source = new BitmapImage(new Uri(commanderDir + FileData.hubBeforeCommanderInfo[beforeRandNum[2]] + ".png", UriKind.Relative));
            BeforeCommander4.Source = new BitmapImage(new Uri(commanderDir + FileData.hubBeforeCommanderInfo[beforeRandNum[3]] + ".png", UriKind.Relative));
            BeforeCommander5.Source = new BitmapImage(new Uri(commanderDir + FileData.hubBeforeCommanderInfo[beforeRandNum[4]] + ".png", UriKind.Relative));
            BeforeCommander6.Source = new BitmapImage(new Uri(commanderDir + FileData.hubBeforeCommanderInfo[beforeRandNum[5]] + ".png", UriKind.Relative));

            AfterCommander1.Source = new BitmapImage(new Uri(commanderDir + FileData.hubAfterCommanderInfo[afterRandNum[0]] + ".png", UriKind.Relative));
            AfterCommander2.Source = new BitmapImage(new Uri(commanderDir + FileData.hubAfterCommanderInfo[afterRandNum[1]] + ".png", UriKind.Relative));
            AfterCommander3.Source = new BitmapImage(new Uri(commanderDir + FileData.hubAfterCommanderInfo[afterRandNum[2]] + ".png", UriKind.Relative));
            AfterCommander4.Source = new BitmapImage(new Uri(commanderDir + FileData.hubAfterCommanderInfo[afterRandNum[3]] + ".png", UriKind.Relative));
        }

        //点击自选指挥官事件响应
        private void Commander_MouseDown(object sender, RoutedEventArgs e)
        {
            Warn.Text = "";
            Image selectCommander = (Image)sender;
            if (selectCommander != null && hasSelectCommander.Count < 2 && !hasSelectCommander.Contains(selectCommander))
            {
                hasSelectCommander.Add(selectCommander);
            }
            FlashHasSelectCommander();
        }

        //取消当前选择的指挥官事件响应
        private void CancelCommander_MouseDown(Object sender, RoutedEventArgs e)
        {
            Image cancelCommander = (Image)sender;
            if (cancelCommander != null)
            {
                for (int i = 0; i < hasSelectCommander.Count; i++)
                {
                    if (hasSelectCommander[i] != null
                        && hasSelectCommander[i].Source.ToString().Equals(cancelCommander.Source.ToString()))
                    {
                        hasSelectCommander.RemoveAt(i);
                    }
                }
            }
            else { return; }
            FlashHasSelectCommander();
        }

        //刷新选择的指挥官
        private void FlashHasSelectCommander()
        {
            if (hasSelectCommander != null)
            {
                HasSelectCommander1.Source = hasSelectCommander.Count < 1 ? null : hasSelectCommander[0].Source;
                HasSelectCommander2.Source = hasSelectCommander.Count < 2 ? null : hasSelectCommander[1].Source;
            }
        }


        //确认按钮事件响应
        private void Button_Confirm_Click(object sender, RoutedEventArgs e)
        {
            botName = IsRandAIFunc();
            if (hasSelectFactor != null)
            {
                if (_modeName.Equals(Dictionary.I18n.Lang.ResourceManager.GetString("SixMutatorsMode")) && hasSelectFactor != null && hasSelectFactor.Count != 3)
                {
                    Warn.Text = Dictionary.I18n.Lang.ResourceManager.GetString("FreeMutatorsWarn4");
                    return;
                }
                else if (_modeName.Equals(Dictionary.I18n.Lang.ResourceManager.GetString("EightMutatorsMode")) && hasSelectFactor != null && hasSelectFactor.Count != 5)
                {
                    Warn.Text = Dictionary.I18n.Lang.ResourceManager.GetString("FreeMutatorsWarn3");
                    return;
                }
            }

            if (hasSelectCommander == null || hasSelectCommander.Count < 2)
            {
                Warn.Text = Dictionary.I18n.Lang.ResourceManager.GetString("CommanderWarnHub");
                return;
            }
            else
            {
                Warn.Text = "";
            }

            this.NavigationService.Navigate(new ShowHubDetail(this));
        }



        //地图相关控件的可用性设置
        private void SetRandMapEnable(bool enable)
        {
            MapImg1.IsEnabled = enable;
            MapImg2.IsEnabled = enable;
            MapImg3.IsEnabled = enable;
            HasSelectMap.IsEnabled = enable;
            RandStartBtn.IsEnabled = enable;
            MapConfirmBtn.IsEnabled = enable;
            BaseConfirmBtn.IsEnabled = true;
        }

        //基础因子相关控件的可用性设置
        private void SetBaseFactorEnable(bool enable)
        {

            NegativeFactor1.IsEnabled = enable;
            NegativeFactor2.IsEnabled = enable;
            NegativeFactor3.IsEnabled = enable;
            NegativeFactor4.IsEnabled = enable;
            NegativeFactor5.IsEnabled = enable;
            NegativeFactor6.IsEnabled = enable;
            NegativeFactor7.IsEnabled = enable;
            NegativeFactor8.IsEnabled = enable;
            NegativeFactor9.IsEnabled = enable;
            NegativeFactor10.IsEnabled = enable;
            NegativeFactor11.IsEnabled = enable;
            NegativeFactor12.IsEnabled = enable;
            NegativeFactor13.IsEnabled = enable;
            NegativeFactor14.IsEnabled = enable;

            MultiFactor1.IsEnabled = enable;
            MultiFactor2.IsEnabled = enable;
            MultiFactor3.IsEnabled = enable;
            MultiFactor4.IsEnabled = enable;
            MultiFactor5.IsEnabled = enable;
            MultiFactor6.IsEnabled = enable;
            MultiFactor7.IsEnabled = enable;
            MultiFactor8.IsEnabled = enable;
            MultiFactor9.IsEnabled = enable;

            HasSelectBaseFactor1.IsEnabled = enable;
            HasSelectBaseFactor2.IsEnabled = enable;
            HasSelectBaseFactor3.IsEnabled = enable;

            BaseConfirmBtn.IsEnabled = enable;
        }

        //固定因子清除
        private void ClearFixedFactor()
        {
            //相对路径URI指定因子图片来源
            FixedFactor1.Source = new BitmapImage();
            FixedFactor2.Source = new BitmapImage();
        }

        //正面因子清除
        private void ClearBaseNegativeFactor()
        {
            //相对路径URI指定因子图片来源
            NegativeFactor1.Source = new BitmapImage();
            NegativeFactor2.Source = new BitmapImage();
            NegativeFactor3.Source = new BitmapImage();
            NegativeFactor4.Source = new BitmapImage();
            NegativeFactor5.Source = new BitmapImage();
            NegativeFactor6.Source = new BitmapImage();
            NegativeFactor7.Source = new BitmapImage();
            NegativeFactor8.Source = new BitmapImage();
            NegativeFactor9.Source = new BitmapImage();
            NegativeFactor10.Source = new BitmapImage();
            NegativeFactor11.Source = new BitmapImage();
            NegativeFactor12.Source = new BitmapImage();
            NegativeFactor13.Source = new BitmapImage();
            NegativeFactor14.Source = new BitmapImage();
            NegativeFactor15.Source = new BitmapImage();
        }

        //多线因子清除
        private void ClearBaseMultiFactor()
        {
            //相对路径URI指定因子图片来源
            MultiFactor1.Source = new BitmapImage();
            MultiFactor2.Source = new BitmapImage();
            MultiFactor3.Source = new BitmapImage();
            MultiFactor4.Source = new BitmapImage();
            MultiFactor5.Source = new BitmapImage();
            MultiFactor6.Source = new BitmapImage();
            MultiFactor7.Source = new BitmapImage();
            MultiFactor8.Source = new BitmapImage();
            MultiFactor9.Source = new BitmapImage();
        }

        //自选因子清除
        private void ClearRandomFactor()
        {
            //相对路径URI显示5个因子的图片
            SelectFactor1.Source = new BitmapImage();
            SelectFactor2.Source = new BitmapImage();
            SelectFactor3.Source = new BitmapImage();
            SelectFactor4.Source = new BitmapImage();
            SelectFactor5.Source = new BitmapImage();
            SelectFactor6.Source = new BitmapImage();
            SelectFactor7.Source = new BitmapImage();
            SelectFactor8.Source = new BitmapImage();
        }

        //随机先出和后出指挥官清除显示
        private void clearRandomCommander()
        {
            //相对路径URI指定指挥官图片来源
            BeforeCommander1.Source = new BitmapImage();
            BeforeCommander2.Source = new BitmapImage();
            BeforeCommander3.Source = new BitmapImage();
            BeforeCommander4.Source = new BitmapImage();
            BeforeCommander5.Source = new BitmapImage();
            BeforeCommander6.Source = new BitmapImage();

            AfterCommander1.Source = new BitmapImage();
            AfterCommander2.Source = new BitmapImage();
            AfterCommander3.Source = new BitmapImage();
            AfterCommander4.Source = new BitmapImage();
        }





        //实现绑定响应接口
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
