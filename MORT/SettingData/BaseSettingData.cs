using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MORT.SettingData
{

    public interface ISettingDataParse
    {
        string ToSave();
        void LoadValue(string fileData);

    }

    public interface ISettingData<T> : ISettingDataParse
    {
        T Value { get; set; }
    }

    public static class SettingDataFactory
    {
        public static ISettingData<T> Create<T>(string key, List<ISettingDataParse> parList)
        {
            if(typeof(T) == typeof(bool))
            {
                var result = (ISettingData<T>)new BoolSettingData(key);
                parList.Add(result);
                return result;
            }

            throw new InvalidOperationException();
        }
    }

    public class BaseSettingData<T> : ISettingData<T>
    {
        public T Value { get; set; }
        protected string FileKey { get; set; }
        public string ToSave()
        {
            return $"{FileKey}[{Value.ToString()}]{System.Environment.NewLine}";
        }     

        public virtual void LoadValue(string fileData)
        {

        }
    }

    public class BoolSettingData : BaseSettingData<bool>
    {
        public BoolSettingData(string key)
        {
            FileKey = key;
        }

        public override void LoadValue(string fileData)
        {
            base.LoadValue(fileData);
            Value = Util.ParseBool(fileData, FileKey);
        }
    }


}
