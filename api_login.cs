// Cách kết nối tới api của 2FACloud để đăng nhập thông qua tài khoảng 2FACloud
// Hiện chỉ hỗ trợ c#

//
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

// Khai báo biến lưu trữ địa chỉ api login
private const string ApiUrl = "http://2facloud.mywebcommunity.org/login_api.php"; 

// Dữ liệu người dùng để gửi đến API
var loginData = new { username = "YOUR_USERNAME", password = "YOUR_PASSWORD"}; 

	using (HttpClient client = new HttpClient())
	{
		try
		{
			// Chuyển đổi dữ liệu sang chuỗi JSON
			string jsonData = System.Text.Json.JsonSerializer.Serialize(loginData);

			// Tạo nội dung yêu cầu HTTP
			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

			// Gửi yêu cầu POST đến API
			HttpResponseMessage response = await client.PostAsync(ApiUrl, content);

			// Đọc nội dung phản hồi từ API
			string responseBody = await response.Content.ReadAsStringAsync();

			// Kiểm tra xem nội dung phản hồi có phải là JSON hợp lệ hay không
			if (response.IsSuccessStatusCode)
			{
				// Thử phân tích JSON phản hồi
				try
				{
					var result = System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(responseBody);
					if (result.message == "Login successful")
					{
						MessageBox.Show($"Đăng nhập thành công. Token: {result.token}");
					}
					else
					{
						MessageBox.Show("Đăng nhập không thành công: " + result.message);
					}
				}
				catch (JsonException)
				{
					// Nếu không phải JSON hợp lệ, xử lý thông báo lỗi từ API
					if (responseBody.Contains("<"))
					{
						// Phản hồi có ký tự '<', cho biết có lỗi không phải JSON
						MessageBox.Show("Lỗi từ API: Phản hồi không hợp lệ");
					}
					else
					{
						// Xử lý phản hồi khác không phải JSON
						MessageBox.Show("Lỗi từ API: " + responseBody);
					}
				}
			}
			else
			{
				MessageBox.Show("Lỗi khi gửi yêu cầu đăng nhập: " + response.StatusCode);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Lỗi: " + ex.Message);
		}
	}
}



// Lớp để phân tích JSON phản hồi từ API
public class ApiResponse
{
	public string message { get; set; }
	public string token { get; set; }
}




