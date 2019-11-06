window.onload=function(){
    const round = 2 * this.Math.PI;
    let allCanvas = this.document.getElementsByClassName("graph");
    for(let index in allCanvas){
        if(!allCanvas.hasOwnProperty(index)) continue;
        const _graph = allCanvas[index];
        _graph.height = _graph.nextElementSibling.getBoundingClientRect().height;
        let parent = _graph.closest(".headers");
        let total = Math.floor(parent.querySelector(".stat-total").innerText);
        let graph = _graph.getContext("2d");
        let [passed, failed, skipped] = [
            ElementValuetoNumber(".stat-passed"),
            ElementValuetoNumber(".stat-failed"),
            ElementValuetoNumber(".stat-skipped")
        ]
        const halfwidth = _graph.width/2;
        const halfheight = _graph.height/2;
        const quarterwidth = halfwidth/2;
        drawSector("#6ba34a", 0, passed);
        drawSector("#ea5440", passed, passed+failed);
        drawSector("#ababab", passed+failed, round);

        function ElementValuetoNumber(id){
            return round*Math.floor(parent.querySelector(id).innerText)/total;
        }
        function drawSector(color, start, end){
            graph.beginPath();
            graph.lineWidth = halfwidth;
            graph.lineCap = "butt";
            graph.strokeStyle = color;
            graph.arc(halfwidth, halfheight, quarterwidth, start, end);
            graph.stroke();
        }
    }
}