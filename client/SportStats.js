function getStat() {
  var request = new XMLHttpRequest();
  var firstName = document.getElementById('first').value;
  var lastName = document.getElementById('last').value;;

  var url = "http://127.0.0.1:8083/Golf/TotalEarnings?firstName=" + firstName + "&lastName=" + lastName;

  request.onreadystatechange = function() {
    if (request.readyState == 4 && request.status == 200) {
      var response = JSON.parse(request.responseText);
      var statValue = response.Stat.Fields[0];
      document.getElementById("stat").innerHTML = statValue;
    }
  };

  request.open("GET", url, true);
  request.send();
}
