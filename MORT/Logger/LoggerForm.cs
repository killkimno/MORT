using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MORT.Logger
{
    public partial class LoggerForm : Form
    {
        public LoggerForm()
        {
            InitializeComponent();
            SubscribeLogger();
            RefreshAll();
        }

        // 폼이 닫힐 때 이벤트 구독 해제
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            UnsubscribeLogger();
            base.OnFormClosed(e);
        }

        private void SubscribeLogger()
        {
            var logger = Logger.Instance;
            logger.LogAdded += Logger_LogAdded;
            logger.OcrCountIncreased += Logger_OcrCountIncreased;
            logger.TransCountIncreased += Logger_TransCountIncreased;
        }

        private void UnsubscribeLogger()
        {
            var logger = Logger.Instance;
            logger.LogAdded -= Logger_LogAdded;
            logger.OcrCountIncreased -= Logger_OcrCountIncreased;
            logger.TransCountIncreased -= Logger_TransCountIncreased;
        }

        private void Logger_LogAdded(object? sender, Logger.LogEventArgs e)
        {
            SafeInvoke(() =>
            {
                // 새 로그를 끝에 추가
                if(string.IsNullOrEmpty(richTextBox1.Text))
                    richTextBox1.Text = e.Message;
                else
                    richTextBox1.AppendText(Environment.NewLine + e.Message);

                ScrollRichTextBoxToBottom();
            });
        }

        private void Logger_OcrCountIncreased(object? sender, Logger.CountChangedEventArgs e)
        {
            RefreshCountsSafe();
        }

        private void Logger_TransCountIncreased(object? sender, Logger.CountChangedEventArgs e)
        {
            RefreshCountsSafe();
        }

        private void RefreshAll()
        {
            SafeInvoke(() =>
            {
                richTextBox1.Text = string.Join(Environment.NewLine, Logger.Instance.LogList);
                ScrollRichTextBoxToBottom();
                RefreshCounts();
            });
        }

        private void RefreshCountsSafe()
        {
            SafeInvoke(RefreshCounts);
        }

        private void RefreshCounts()
        {
            richTextBox2.Text = $"OCR Count: {Logger.Instance.OcrTryCount}{Environment.NewLine}Trans Count: {Logger.Instance.TransCount}";
        }

        private void ScrollRichTextBoxToBottom()
        {
            // caret 이동 후 스크롤
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;
            richTextBox1.ScrollToCaret();
        }

        private void SafeInvoke(Action action)
        {
            if(InvokeRequired)
                BeginInvoke(action);
            else
                action();
        }
    }

    // Logger 클래스는 변경없음 (생략 가능)
    public class Logger
    {
        // 내부 데이터
        private readonly List<string> logList = new List<string>();
        private int ocrTryCount;
        private int transCount;

        // 로그 인덱스 카운터 (스레드 안전하게 증가)
        private int logCounter;

        // 싱글톤 인스턴스 (static 메서드가 내부 상태를 변경할 수 있도록)
        private static readonly Logger _instance = new Logger();
        public static Logger Instance => _instance;

        // 이벤트 인자
        public class LogEventArgs : EventArgs
        {
            public string Message { get; }
            public LogEventArgs(string message) => Message = message;
        }

        public class CountChangedEventArgs : EventArgs
        {
            public int NewValue { get; }
            public CountChangedEventArgs(int newValue) => NewValue = newValue;
        }

        // 이벤트: 스트링 추가, ocr 카운트 업, trans 카운트 업
        public event EventHandler<LogEventArgs>? LogAdded;
        public event EventHandler<CountChangedEventArgs>? OcrCountIncreased;
        public event EventHandler<CountChangedEventArgs>? TransCountIncreased;

        // 외부 접근용 읽기 전용
        public IReadOnlyList<string> LogList => logList.AsReadOnly();
        public int OcrTryCount => ocrTryCount;
        public int TransCount => transCount;

        // 로그 추가 (static wrapper)
        public static void AddLog(string message)
        {
            if(message == null) throw new ArgumentNullException(nameof(message));
            _instance.AddLogInternal(message);
        }

        private void AddLogInternal(string message)
        {
            // 인덱스 생성 (1부터 시작)
            var index = System.Threading.Interlocked.Increment(ref logCounter);
            var formatted = $"[{index}] {message}";

            lock(logList)
            {
                logList.Add(formatted);
            }
            LogAdded?.Invoke(this, new LogEventArgs(formatted));
        }

        // OCR/Trans 카운트 증가 - static wrapper
        public static void IncrementOcr()
        {
            _instance.IncrementOcrInternal();
        }

        // Trans 카운트 증가 - static wrapper
        public static void IncrementTrans()
        {
            _instance.IncrementTransInternal();
        }

        // 내부 인스턴스 구현 (직접 호출 금지)
        private void IncrementOcrInternal()
        {
            var newVal = System.Threading.Interlocked.Increment(ref ocrTryCount);
            OcrCountIncreased?.Invoke(this, new CountChangedEventArgs(newVal));
        }

        private void IncrementTransInternal()
        {
            var newVal = System.Threading.Interlocked.Increment(ref transCount);
            TransCountIncreased?.Invoke(this, new CountChangedEventArgs(newVal));
        }

        // 선택적 인스턴스 메서드 유지
        public void IncrementOcrInstance() => IncrementOcrInternal();
        public void IncrementTransInstance() => IncrementTransInternal();
    }
}
