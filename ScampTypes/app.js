var Greeter = (function () {
    function Greeter(element) {
        this.element = element;
        this.element.innerHTML += "The time is: ";
        this.span = document.createElement('span');
        this.element.appendChild(this.span);
        this.span.innerText = new Date().toUTCString();
    }
    Greeter.prototype.start = function () {
        var _this = this;
        this.timerToken = setInterval(function () { return _this.span.innerHTML = new Date().toUTCString(); }, 500);
    };
    Greeter.prototype.stop = function () {
        clearTimeout(this.timerToken);
    };
    return Greeter;
})();
function addText(o) {
    var td = document.createElement("td");
    td.appendChild(document.createTextNode(o));
    return td;
}
function addHead(o) {
    var th = document.createElement("th");
    th.appendChild(document.createTextNode(o));
    return th;
}
function addTable(obj) {
    var id = 0;
    while (document.getElementById("table_" + id) != null || document.getElementById("div_" + id) != null)
        id++;
    var t = document.createElement("table");
    t.id = "table_" + id;
    t.border = "1";
    var tb = document.createElement("tbody");
    t.appendChild(tb);
    var hdr = document.createElement("h2");
    hdr.textContent = obj.constructor.name;
    var div = document.createElement("div");
    div.id = "div_" + id;
    div.appendChild(hdr);
    div.appendChild(t);
    document.body.appendChild(div);
    var thead = document.createElement("thead");
    thead.appendChild(addHead("Name"));
    thead.appendChild(addHead("Type"));
    thead.appendChild(addHead("Value"));
    t.appendChild(thead);
    var keys = Object.keys(obj);
    for (var i = 0; i < keys.length; i++) {
        var key = keys[i];
        var type = obj[key].constructor.name;
        var value = obj[key];
        var tr = document.createElement("tr");
        tr.appendChild(addText(key));
        tr.appendChild(addText(type));
        tr.appendChild(addText(value));
        tb.appendChild(tr);
    }
}
window.onload = function () {
    var s1 = new Messages.Subscribe("1234");
    addTable(s1);
    var u1 = new Messages.Update(1 /* StateChange */, "user", "resource", "state", new Date());
    addTable(u1);
    var e1 = new Messages.Error("bad error");
    addTable(e1);
};
//# sourceMappingURL=app.js.map