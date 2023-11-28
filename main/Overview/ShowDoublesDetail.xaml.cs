using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using main.Helper;

namespace main
{
    /// <summary>
    /// ShowDoublesDetail.xaml 的交互逻辑
    /// </summary>
    public partial class ShowDoublesDetail : Page
    {
        private DoublesModeWindow m_parent;
        public ShowDoublesDetail(DoublesModeWindow parent)
        {
            m_parent = parent;
            InitializeComponent();
            showSelect();
        }

        public ShowDoublesDetail()
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
            HasSelectBaseFactor1.Source = m_parent.HasSelectBaseFactor1.Source;
            HasSelectBaseFactor2.Source = m_parent.HasSelectBaseFactor2.Source;
            HasSelectBaseFactor3.Source = m_parent.HasSelectBaseFactor3.Source;
            FixedMutator1.Source = m_parent.FixedFactor1.Source;
            string BaseFactor1_name = (HasSelectBaseFactor1.Source as BitmapImage).UriSource.ToString().Replace("/main;component/Resources/factor/", "").Replace(".png", "");
            HasSelectBaseFactor1_index.Content = FileData.mutator_index_dict[BaseFactor1_name];
            string BaseFactor2_name = (HasSelectBaseFactor2.Source as BitmapImage).UriSource.ToString().Replace("/main;component/Resources/factor/", "").Replace(".png", "");
            HasSelectBaseFactor2_index.Content = FileData.mutator_index_dict[BaseFactor2_name];

            if (HasSelectBaseFactor3.Source == null)
            {
                HasSelectBaseFactor3_index.Content = "";
            }
            else
            {
                string BaseFactor3_name = (HasSelectBaseFactor3.Source as BitmapImage).UriSource.ToString().Replace("/main;component/Resources/factor/", "").Replace(".png", "");
                HasSelectBaseFactor3_index.Content = FileData.mutator_index_dict[BaseFactor3_name];
            }

            string fixed_mutator_name = (FixedMutator1.Source as BitmapImage).UriSource.ToString().Replace("/main;component/Resources/factor/", "").Replace(".png", "");
            FixedFactor_index.Content = FileData.mutator_index_dict[fixed_mutator_name];
            HasSelectFactor1.Source = m_parent.HasSelectFactor1.Source;
            HasSelectFactor2.Source = m_parent.HasSelectFactor2.Source;
            HasSelectFactor3.Source = m_parent.HasSelectFactor3.Source;
            HasSelectFactor4.Source = m_parent.HasSelectFactor4.Source;
            HasSelectFactor5.Source = m_parent.HasSelectFactor5.Source;
            string SelectFactor1_name = (HasSelectFactor1.Source as BitmapImage).UriSource.ToString().Replace("/main;component/Resources/factor/", "").Replace(".png", "");
            HasSelectFactor1_index.Content = FileData.mutator_index_dict[SelectFactor1_name];
            string SelectFactor2_name = (HasSelectFactor2.Source as BitmapImage).UriSource.ToString().Replace("/main;component/Resources/factor/", "").Replace(".png", "");
            HasSelectFactor2_index.Content = FileData.mutator_index_dict[SelectFactor2_name];

            if (HasSelectFactor3.Source == null)
            {
                HasSelectFactor3_index.Content = "";
            }
            else
            {
                string HasSelectFactor3_name = (HasSelectFactor3.Source as BitmapImage).UriSource.ToString().Replace("/main;component/Resources/factor/", "").Replace(".png", "");
                HasSelectFactor3_index.Content = FileData.mutator_index_dict[HasSelectFactor3_name];
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

            HasSelectCommander1.Source = m_parent.HasSelectCommander1.Source;
            HasSelectCommander2.Source = m_parent.HasSelectCommander2.Source;

            AIBox.Text = m_parent.botName;
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
