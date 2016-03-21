google.charts.load("current", { packages: ["corechart", "bar"] });
google.charts.setOnLoadCallback(drawChart);


function drawChart() {
    var below = +document.getElementById('BelowId').value;
    var tie = +document.getElementById('TieId').textContent;
    var over = +document.getElementById('OverId').textContent;
    var data = google.visualization.arrayToDataTable([
      ['Task', 'Hours per Day'],
      ['הערכת צנועה', below],
      ['הערכה מדוייקת', tie],
      ['הערכת יתר', over]
    ]);

    //FE2E2E  5882FA  81F781
    var options = {
        title: '',
        pieHole: 0.4,
        colors: ['#81F781', '#5882FA', '#FE2E2E']
    };

    var chart = new google.visualization.PieChart(document.getElementById('donutchart'));
    chart.draw(data, options);
}