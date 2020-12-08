let express = require('express');
let path = require('path');
let routes = require('./routes');

let app = express();
app.set("port", process.env.PORT || 8080 );
app.use(express.static(path.join(__dirname, '/views')));
app.use(routes);

app.listen(app.get("port"), function(){
    console.log("Server started on port: " + app.get("port"));
});