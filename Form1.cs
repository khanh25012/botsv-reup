using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;

using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.Data.SqlClient;
using System.Data;

namespace botKMT
{
    public partial class Form1 : Form
    {

        TelegramBotClient botClient;


        int logCounter = 0;

        void AddLog(string msg)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.BeginInvoke((MethodInvoker)delegate ()
                {
                    AddLog(msg);
                });
            }
            else
            {
                logCounter++;
                if (logCounter > 100)
                {
                    txtLog.Clear();
                    logCounter = 0;
                }
                txtLog.AppendText(msg + "\r\n");
            }
        }

        /// <summary>
        /// hàm tạo: ko kiểu, trùng tên với class
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            string token = "5800903209:AAEL6XLVnA90S8cFcioVrfZwqqswIs9io5k";

            //Console.WriteLine("my token=" + token);

            botClient = new TelegramBotClient(token);

            using CancellationTokenSource cts = new();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,  //hàm xử lý khi có người chát đến
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
            Task<User> me = botClient.GetMeAsync();

            AddLog($"Bot begin working: @{me.Result.Username}");

            //async lập trình bất đồng bộ
            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                // Only process Message updates: https://core.telegram.org/bots/api#message
                bool ok = false;

                //kdl? biến <=> biến đó có thể nhận NULL

                Telegram.Bot.Types.Message? message = null;

                if (update.Message != null)
                {
                    message = update.Message;
                    ok = true;
                }

                if (update.EditedMessage != null)
                {
                    message = update.EditedMessage;
                    ok = true;
                }

                if (!ok || message == null) return; //thoát ngay

                string? messageText = message.Text;
                if (messageText == null) return;  //ko chơi với null

                var chatId = message.Chat.Id;  //id của người chát với bot

                AddLog($"{chatId}: {messageText}");  //show lên để xem
                string reply = "";  //đây là text trả lời

                string s2 = messageText.ToLower();

                if (s2.StartsWith("/cmd"))
                {
                    // /cmd dir d:\
                    // 012345678901
                    string cmd = messageText.Substring(5);
                    ClassCMD.RUN(cmd);
                    while (!ClassCMD.done)
                    {
                        //đợi chạy xong
                        Application.DoEvents(); //chống treo
                    }
                    reply = ClassCMD.kq;
                }
                else if (s2.StartsWith("/cp "))
                {
                    string path = messageText.Substring(4);

                    string fn = System.IO.Path.GetFileName(path);
                    reply = $"Đã gửi file {fn} qua telegram rồi, click vào mũi tên để lưu về máy";

                    await using Stream stream = System.IO.File.OpenRead(path);
                    Telegram.Bot.Types.Message fileMessage = await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: InputFile.FromStream(stream: stream, fileName: fn),
                        caption: $"Download file {path}");
                }

                else if (s2.StartsWith("tknv "))
                {
                    //tknv Đông Sơn
                    //01234567890
                    string q = messageText.Substring(5);
                    reply = clsTimKiemDB.TimKiem(q);
                }
                else
                {
                    reply = "để mị nói cho mà nghe: " + messageText;
                }
                AddLog(reply); //show log to see

                // Echo received message text
                Telegram.Bot.Types.Message sentMessage = await botClient.SendTextMessageAsync(
                       chatId: chatId,
                       text: reply,
                       parseMode: ParseMode.Html  //bổ xung ngày 25.5.2023
                      );

                //đọc thêm về ParseMode.Html tại: https://core.telegram.org/bots/api#html-style
            }

            Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                AddLog(ErrorMessage);
                return Task.CompletedTask;
            }
        }

        const long idHacker = 1875746636; //thay id của bạn vào
        private void cmd1_Click(object sender, EventArgs e)
        {
            //gửi đi nội dung trên txt1
            botClient.SendTextMessageAsync(
                    chatId: idHacker,
                    text: txt1.Text);
        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

public class clsTimKiemDB
{
    static string cnStr = "Server=DESKTOP-2HC9Q42\\TEST01;Database=BTL;Trusted_Connection=True;TrustServerCertificate=True;";

    public static string TimKiem(string q)
    {
        string kq = "";
        try
        {
            using (SqlConnection cn = new SqlConnection(cnStr))
            {
                cn.Open();
                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandText = "SP_Search_NV";
                    cm.CommandType = CommandType.StoredProcedure;
                    cm.Parameters.Add("@q", SqlDbType.NVarChar, 50).Value = "%" + q.Replace(' ', '%') + "%";
                    object obj = cm.ExecuteScalar(); //lấy col1 of row1
                    if (obj != null)
                        kq = (string)obj;
                    else
                        kq = "không có dữ liệu";
                }
            }
        }
        catch (Exception ex)
        {
            kq += $"Error: {ex.Message}";
        }
        return kq;
    }
}