using main.Helper;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace main
{
    /// <summary>
    /// ShowSingleDetail.xaml 的交互逻辑
    /// </summary>
    public partial class ShowSingleDetail : Page
    {
        private SingleModeWindow m_parent;
        public ShowSingleDetail(SingleModeWindow parent)
        {
            m_parent = parent;
            InitializeComponent();
            showSelect();
        }

        public ShowSingleDetail()
        {
            InitializeComponent();
        }

        //展示已选择的因子
        private void showSelect()
        {
            //上个窗口的数据传递至本窗口
            PlayerName.Text = m_parent.PlayerName.Text;
            modeName.Text = m_parent.modeName;

            HasSelectMap.Source = m_parent.HasSelectMap.Source;
            MapTip.Text = m_parent.MapTip.Text;

            HasSelectFactor1.Source = m_parent.HasSelectFactor1.Source;
            HasSelectFactor2.Source = m_parent.HasSelectFactor2.Source;
            HasSelectFactor3.Source = m_parent.HasSelectFactor3.Source;
            HasSelectFactor4.Source = m_parent.HasSelectFactor4.Source;
            HasSelectFactor5.Source = m_parent.HasSelectFactor5.Source;

            string SelectFactor1_name = (HasSelectFactor1.Source as BitmapImage).UriSource.ToString().Replace("/main;component/Resources/factor/", "").Replace(".png", "");
            HasSelectFactor1_index.Content = FileData.mutator_index_dict[SelectFactor1_name];
            string SelectFactor2_name = (HasSelectFactor2.Source as BitmapImage).UriSource.ToString().Replace("/main;component/Resources/factor/", "").Replace(".png", "");
            HasSelectFactor2_index.Content = FileData.mutator_index_dict[SelectFactor2_name];
            string SelectFactor3_name = (HasSelectFactor3.Source as BitmapImage).UriSource.ToString().Replace("/main;component/Resources/factor/", "").Replace(".png", "");
            HasSelectFactor3_index.Content = FileData.mutator_index_dict[SelectFactor3_name];

            if (HasSelectFactor5.Source == null)
            {
                HasSelectFactor5_index.Content = "";
            }
            else
            {
                string HasSelectFactor5_name = (HasSelectFactor5.Source as BitmapImage).UriSource.ToString().Replace("/main;component/Resources/factor/", "").Replace(".png", "");
                HasSelectFactor5_index.Content = FileData.mutator_index_dict[HasSelectFactor5_name];
            }

            if (HasSelectFactor4.Source == null)
            {
                HasSelectFactor4_index.Content = "";
            }
            else
            {
                string HasSelectFactor4_name = (HasSelectFactor4.Source as BitmapImage).UriSource.ToString().Replace("/main;component/Resources/factor/", "").Replace(".png", "");
                HasSelectFactor4_index.Content = FileData.mutator_index_dict[HasSelectFactor4_name];
            }



            Score.Text = m_parent.Score.Text;

            HasSelectCommander.Source = m_parent.HasSelectCommander.Source;

            AIBox.Text = m_parent.botName;
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
