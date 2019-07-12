using System;
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
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace readifc
{
    public class User
    {       
        /// 编号       
        public string  ID { get; set; }     
        /// 姓名 
        public string Name { get; set; }       
        /// 年龄        
        public string Property { get; set; }
             /// 性别       
        public string Value { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
    public class UserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (propertyName != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    public class ShowDataViewModel : UserViewModel
    {
        //数据源
        ObservableCollection<User> _mylist = new ObservableCollection<User>();
        public ObservableCollection<User> mylist
        {

            get { return _mylist; }
            set
            {
                _mylist = value;
                RaisePropertyChanged("mylist");
            }
        }
        //构造函数
        public ShowDataViewModel()
        {
            const string fileName = "rvt17.ifc";
            using (var model = IfcStore.Open(fileName))
            {
                var ifcsites = model.Instances.OfType<IIfcSite>();

                // 遍历所有的墙 
                foreach (IIfcSite ifcsite in ifcsites)
                {
                    var properties = ifcsite.IsDefinedBy
                   .Where(r => r.RelatingPropertyDefinition is IIfcPropertySet)
                   .SelectMany(r => ((IIfcPropertySet)r.RelatingPropertyDefinition).HasProperties)
                   .OfType<IIfcPropertySingleValue>();
                    foreach (var property in properties)
                    {                      
                        mylist.Add(new User() { ID = ifcsite.GlobalId, Name = ifcsite.Name, Property = property.Name, Value = property.NominalValue.ToString(), Remarks = "无" });
                    }
                }          
            }           
        }      

        public ShowDataViewModel(ObservableCollection<User> mylist)
        {
            _mylist = mylist;
        }
    }

}
