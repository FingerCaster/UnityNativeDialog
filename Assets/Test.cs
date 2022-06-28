using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NativeDialogs.Runtime;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace NativeDialog
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private Button m_Button1;
        [SerializeField] private Button m_Button2;
        [SerializeField] private Button m_Button3;
        [SerializeField] private Button m_Button4;
        [SerializeField] private Button m_Button5;
        [SerializeField] private Text m_Text;
  
        private void Start()
        {
       
            // Debug.Log("1--------------------------");
            //
            // Debug.Log(Process.GetCurrentProcess());
            // Debug.Log("2--------------------------");
            // Debug.Log(Process.GetCurrentProcess().MainModule == null);
            // Debug.Log("3--------------------------");
            //
            // Debug.Log(Process.GetCurrentProcess().MainModule?.ModuleName);
            // Debug.Log("4--------------------------");

            m_Button1.onClick.AddListener(() =>
            {
                DialogManager.Instance.ShowDialog("测试", "测试信息", _ =>
                {
                    m_Text.text = $"对话框返回结果:{_}";
                });
            });
            m_Button2.onClick.AddListener(() =>
            {
                DialogManager.Instance.ShowDialog("测试", "测试信息", "确定1",_ =>
                {
                    m_Text.text = $"对话框返回结果:{_}";
                });
            });
            m_Button3.onClick.AddListener(() =>
            {
                DialogManager.Instance.ShowDialog("测试", "测试信息", "取消1","确定2",_ =>
                {
                    m_Text.text = $"对话框返回结果:{_}";
                });
            });
            m_Button4.onClick.AddListener(() =>
            {
                DialogManager.Instance.ShowDialog("测试", "测试信息","取消1","确定2","其他3", _ =>
                {
                    m_Text.text = $"对话框返回结果:{_}";
                });
            });
            m_Button5.onClick.AddListener(() =>
            {
                DialogManager.Instance.ShowDialog(null, "测试信息","取消1","确定2","其他3", _ =>
                {
                    m_Text.text = $"对话框返回结果:{_}";
                });
            });
          
        }
    }
}