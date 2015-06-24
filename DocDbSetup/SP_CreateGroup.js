// create a new scamp resource group
function (groupDoc) {
    var context = getContext();
    var collection = context.getCollection();
    var response = context.getResponse();

    // to hold the retrieved documents
    var groupDoc, userDoc;

    // filter queries to be used 
    var userQueryFilter = "SELECT * FROM u where u.id  = '" + groupDoc.budget.ownerId + "' AND u.type = 'user'";

    // get user document
    var accept = collection.queryDocuments(collection.getSelfLink(), userQueryFilter, {},
        function (err, documents, responseOptions) {
            if (err) throw new Error("Error" + err.message);

            if (documents.length != 1) throw "Unable to find user " + groupDoc.budget.ownerId;
            userDoc = documents[0];
            CreateGroup(groupDoc, userDoc);
        });
    if (!accept) throw "Unable to read user document, abort ";

    // add user to group
    function CreateGroup(groupDoc, userDoc) {

        // define user for group list
        var groupFrag = {
            "id": userDoc.id,
            "name": userDoc.name,
            "isManager" : "true"
        };
        // if no empty group membership collection, add one
        if (groupDoc.members == null || typeof(groupDoc.members) == 'undefined' )
            groupDoc.members = {};
        // add the user to the group collection
        groupDoc.members.push(groupFrag);

        // add user's default allocation to the group's allocated amount
        groupDoc.budget.allocated += groupDoc.budget.defaultUserAllocation

        // increment user's allocated budget amount by group budget amount
        userDoc.budget.allocated += groupDoc.budget.unitsBudgeted;

        // define group for group list
        var userFrag = {
            "id": groupDoc.id,
            "name": groupDoc.name
        };
        // add the group to the user membership collection
        userDoc.groupmbrship.push(userFrag);
    
        // perform updates
        var accept = collection.createDocument(collection.getSelfLink(), groupDoc,
            function (err, docReplaced) {
                if (err) throw "Unable to create group document " + groupDoc.id + ", abort ";

                var accept2 = collection.replaceDocument(userDoc._self, userDoc,
                    function (err2, docReplaced2) {
                        if (err) throw "Unable to update user document " + userDoc.id + ", abort"
                    });

                if (!accept2) throw "Unable to update user document " + userDoc.id + ", abort";
            });
        if (!accept) throw "Unable to update group document " + groupDoc.id + ", abort";
    }
}
