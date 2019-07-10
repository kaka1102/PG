
namespace Mobile_Deura.Const
{
    // Model Constants
    public static class SystemMessageConst // Lớp static SystemMessageConst
    {
        public static class systemmessage
        {
            public const string IncorrectCodeActive = "Nhập sai code active";
            public const string IncorrectCodeActiveEn = "Incorrect code active";
            public const string CreateAccountSuccess = "Tạo tài khoản thành công";
            public const string CreateAccountSuccessEn = "Create account successfull";
            public const string AddSuccess = "Thêm thành công";
            public const string AddSuccessEn = "Add successfull";
            public const string BuySuccess = "Thanh toán thành công";
            public const string BuySuccessEn = "Check out successfull";
            public const string SaveSuccess = "Cập nhật thành công";
            public const string SaveSuccessEn = "Update successful";
            public const string IsNotExist = "Dữ liệu không tồn tại";
            public const string IsNotExistEn = "This Data is not existed!";
            public const string EmailIsExisted = "Email đã tồn tại";
            public const string EmailIsExistedEn = "Email is existed";
            public const string PhoneIsExisted = "Số điện thoại đã tồn tại";
            public const string PhoneIsExistedEn = "Phone is existed";
            public const string EmailIsNotExist = "Email không tồn tại";
            public const string EmailIsNotExistEn = "Email is not existed";
            public const string EmailAndPhoneIsExisted = "Email hoặc số điện thoại này đã được đăng ký";
            public const string EmailAndPhoneIsExistedEn = "Email or phone is existed";
            public const string EditSuccess = "Cập nhật thành công";
            public const string EditSuccessEn = "Update successfull";
            public const string ConfirmAfterDelete = "Bạn có chắc chắn muốn xóa !";
            public const string DeleteSuccess = "Xóa thành công";
            public const string DataIsEmpty = "Chưa có dữ liệu";
            public const string WereAreSendMailConfirm = "Đăng ký tài khoản thành công,Mời bạn kiểm tra hòm thư để xác nhận mail";
            public const string SendMailActive = "Mail kích hoạt đã được gửi";
            public const string SendMailActiveEn = "This mail confirm has been send to your mail";
            public const string SendMailForgotPassword= "Mail lấy lại mật khẩu đã được gửi";
            public const string SendMailForgotPasswordEn = "Mail forgotpassword has been send";
            public const string MustCheckAgree = "Bạn chưa chọn đồng ý";
            public const string MustCheckAgreeEn = "You must be check agree";
            public const string EmailConfirmSuccessFull = "Xác nhận email thành công";
            public const string EmailConfirmUnSuccessFull = "Xác nhận email thất bại";
            public const string EmailConfirmIsNotCorrect = "Xác nhận email không chính xác";
            public const string EmailConfirmIsNotCorrectEn = "Email confirm is not correct";
            public const string DateTimeIsNotCorrectFormat = "Sai định dạng ngày tháng";
            public const string ConfirmPasswordNotCorrect = "Xác nhận mật khẩu không chính xác";
            public const string ConfirmPasswordNotCorrectEn = "Confirm password is not correct !";
            public const string OldPasswordNotCorrect = "Mật khẩu cũ không chính xác";
            public const string OldPasswordNotCorrectEn = "Old password is not correct";
            public const string SendSuccess = "Gửi thành công";
            public const string SendSuccessEn = "Send Success";
            public const string StatusHide = "Hide";
            public const string StatusShow = "Show";
            public const string AccountNotExist = "Tài khoảng không tồn tại";
            public const string AccountNotExistEn = "This Account is not exist !";
            public const string PasswordNotCorrect = "Mật khẩu không chính xác";
            public const string PasswordNotCorrectEn = "Your password is not correct !";
            public const string City = "thành phố";
            public const string CityEn = "thành phố";
            public const string Phone = "số điện thoại";
            public const string PhoneEn = "phone";
            public const string Address = "địa chỉ";
            public const string AddressEn = "address";
            public const string Address2 = "địa chỉ 2";
            public const string Address2En = "address2";
            public const string Fullname = "họ và tên";
            public const string FullnameEn = "Fullname";
            public const string MessageEn = "Message";
            public const string Message = "Nội dung tin nhắn";
            public const string DefaultImage = "/Content/dist/img/default-image.gif";
            public const string NoData = "Không có dữ liệu";
            public const string NoDataEn = "No Data";
            public const string CartEmpty = "Giỏ hàng trống !";
            public const string CartEmptyEn = "Your cart is empty !";
            public const string CaptchaIncorrect = "Nhập lại captcha";
            public const string CaptchaIncorrectEn = "Incorrect captcha";
            public const string ArticleExisted = "Mã sản phẩm đã tồn tại";
            public const string PhoneConfirmMes = "Một tin nhắn chứa mã xác nhận đã được gửi đến số của bạn , hãy nhập mã xác nhận đó để hoàn tất đăng ký !";
            public const string PhoneConfirmMesEn = "A message containing a confirmation code has been sent to your number, please enter the confirmation code to complete the registration !";
            public const string BataMessageActiveCode = "Ban dung ma so sau day de hoan tat dang ky : ";
            public const string Notification = "Thông báo";
            public const string NotificationEn = "Notification";
            public const string SearchContentFormat = "Từ khóa không đúng định dạng !";
            public const string SearchContentFormatEn = "This content is not correct format !";

         
        }

        public static class SmsType
        {
            public const string SmsUsernameAndPassword = "Bata xin thong bao tai khoan dang nhap website giaybata.com.vn cua ban la : username : {0} password : {1}.";
        }

        public static class RankConst
        {
            public const int RankA = 7000000;
            public const int RankB = 30000000;
            public const int RankABonus = 5;
            public const int RankBBonus = 10;
            public const int RankABirthBonus = 10;
            public const int RankBBirthBonus = 15;
        }

        // Đối tượng static ko đổi ValidateConst
        public static class ValidateConst
        {
            public const string MinlengthOfText = "Độ dài của {0} phải lớn hơn {1}."; // Phần tử MinlengthOfText truyền vào 2 tham số
            public const string MinlengthOfTextEn = "Length of {0} must be greater {1}.";
            public const string EmailNotCorrectFormat = "Email không đúng định dạng";
            public const string EmailNotCorrectFormatEn = "Email is not correct Format";
            public const string CheckNotEmpty = "{0} không được để trống";
            public const string CheckNotEmptyEn = "{0} is not empty";
            public const string MinMaxlengthOfText = "Độ dài của {0} Phải nhỏ hơn {1} Và  lớn hơn {2}."; // Phần tử MinlengthOfText truyền vào 3 tham số
            public const string MinMaxlengthOfTextEn = "Length of {0} must be smaller {1} and greater {2}.";
            public const string MaxlengthOfText = "Độ dài của {0} Phải nhỏ hơn {1}.";
            public const string MaxlengthOfTextEn = "Length of {0} must be smaller {1}.";
        }

        public static class PrefixOrder
        {
            public const string PrefixPayOnline = "PP-";
            public const string PayLater = "PL-";
        }

        public static class EmailInfomation
        {
            public const string From = "test@test.com";
            public const string Host = "mailtest.com";
            public const int Post = 587;
            public const string Username = "test@test.com";
            public const string Password = "test@";
        }

    }

    public static  class SystemUntility
    {
       
        public static string TruncString(string myStr, int THRESHOLD)
        {
            if(myStr != null)
            if (myStr.Length >= THRESHOLD) return myStr.Substring(0, THRESHOLD) + "...";

            return myStr;
        }


    }

}