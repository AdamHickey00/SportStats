function getStat() {
  var request = new XMLHttpRequest();
  var url = "http://127.0.0.1:8083/Golf/TotalEarnings?firstName=Kevin&lastName=Kisner";

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
