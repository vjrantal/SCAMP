// Add User to Group Stored Proc
//TODO: modify to allow for add/remove
function (group) {
    var context = getContext();
    var collection = context.getCollection();
    var response = context.getResponse();

    // to hold the retrieved documents
    var groupDoc, userDoc;

    // filter queries to be used 
    var userQueryFilter = "SELECT * FROM u where u.id  = '" + userId + "' AND u.type = 'user'";

    // get user document
    var accept2 = collection.queryDocuments(collection.getSelfLink(), userQueryFilter, {},
        function (err2, documents2, responseOptions2) {
            if (err2) throw new Error("Error" + err2.message);
            if (documents2.length != 1) throw "Unable to find user " + userId;
        });

    // build group object
            // get group document
    var accept = collection.queryDocuments(collection.getSelfLink(), groupQueryFilter, {},
        function (err, documents, responseOptions) {
            if (err) throw new Error("Error" + err.message);

            if (documents.length != 1) throw "Unable to find group " + groupId;
            groupDoc = documents[0];

            // get user document
            var accept2 = collection.queryDocuments(collection.getSelfLink(), userQueryFilter, {},
                function (err2, documents2, responseOptions2) {
                    if (err2) throw new Error("Error" + err2.message);
                    if (documents2.length != 1) throw "Unable to find user " + userId;
                    userDoc = documents2[0];
                    addToGroup(groupDoc, userDoc);
                    return;
                });
            if (!accept2) throw "Unable to read user document, abort ";
        });
    if (!accept) throw "Unable to read group document, abort ";

    // add user to group
    function addToGroup(groupDoc, userDoc) {
        // elements to add to each document
        var userFrag = {
            "id": userDoc.id,
            "name": userDoc.name
        };
        var groupFrag = {
            "id": groupDoc.id,
            "name": groupDoc.name
        };

        // update documents
        groupDoc.members.push(userFrag);
        userDoc.groupmbrship.push(groupFrag);

        // perform updates
        var accept = collection.replaceDocument(groupDoc._self, groupDoc,
            function (err, docReplaced) {
                if (err) throw "Unable to update group document " + groupDoc.id + ", abort ";

                var accept2 = collection.replaceDocument(userDoc._self, userDoc,
                    function (err2, docReplaced2) {
                        if (err) throw "Unable to update user document " + userDoc.id + ", abort"
                    });

                if (!accept2) throw "Unable to update user document " + userDoc.id + ", abort";
            });
        if (!accept) throw "Unable to update group document " + groupDoc.id + ", abort";
    }
}
