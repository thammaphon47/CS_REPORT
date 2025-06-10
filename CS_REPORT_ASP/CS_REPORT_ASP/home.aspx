<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="home.aspx.vb" Inherits="CS_REPORT_ASP.home" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Customer Service Dept.</title>
    <style>
body {
    font-family: Arial, sans-serif;
    background-image: url('');
    background-size: cover;
    background-position: center;
    background: #C4E1E6;
    margin: 0;
    padding: 0;
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100vh;
    text-align: center;
}

.container {
    margin-top: 0;
    background-color: white;
    padding: 40px;
    border-radius: 15px;
    box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2), 0 4px 6px rgba(0, 0, 0, 0.1);
}

img.logo {
    width: 250px;
    height: auto;
    margin-bottom: 30px;
}

h1 {
    color: #333;
    margin-bottom: 10px;
}

.btn {
    display: inline-block;
    padding: 10px 25px;
    font-size: 18px;
    background-color: #EB3678;
    color: white;
    text-decoration: none;
    border-radius: 5px;
    margin-top: 20px;
}

.btn:hover {
    background-color: #005a9e;
}
  .title {
            color: blue;
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 30px;
        }

        .btn {
            width: 300px;
            height: 60px;
            font-size: 18px;
            font-weight: bold;
            border-radius: 10px;
            margin: 15px;
            border: none;
            cursor: pointer;
        }

        .delivery {
            background-color: #FEBA17;
            border: 2px solid #cccccc;
        }

        .replacement {
            background-color: #A4B465;
        }

        .close {
            background-color: #EA2F14;
        }
    </style>

</head>
<body>
<form id="form1" runat="server">
    <div class="container">
          <asp:Button ID="btnDeliveryNote" runat="server" Text="DELIVERY NOTE" CssClass="btn delivery" OnClick="btnDeliveryNote_Click" />
        <br />
        <asp:Button ID="btnReplacement" runat="server" Text="REPLACEMENT" CssClass="btn replacement" OnClick="btnReplacement_Click" />
        <br />
    </div>
</form>

</body>
</html>