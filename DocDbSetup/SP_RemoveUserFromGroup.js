// Remove User From Group Stored Proc
// removes the specified user from the group and flags their 
// resources in that group as "deleted"
function (groupId, userId) {
    var context = getContext();
    var collection = context.getCollection();
    var response = context.getResponse();

    // to hold the retrieved documents
    var groupDoc, userDoc;

    // filter queries to be used 
    var groupQueryFilter = "SELECT * FROM g where g.id  = '" + groupId + "' AND g.type = 'group'";
    var userQueryFilter = "SELECT * FROM u where u.id  = '" + userId + "' AND u.type = 'user'";

    // get group and user documents
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
                    removeFromGroup(groupDoc, userDoc);
                    return;
                });
            if (!accept2) throw "Unable to read user document, abort ";
        });
    if (!accept) throw "Unable to read group document, abort ";

    // remove user from group
    function removeFromGroup(groupDoc, userDoc) {

        // user element in membership collection
        for (i = 0; i < groupDoc.members.length; i++) {
            if (groupDoc.members[i].id === userDoc.id) {
                // for each resource the user has in the group, flag them for deletion
                if (typeof(groupDoc.members[i].resources) != 'undefined' && groupDoc.members[i].resources != null)
                {
                    userResourceList = groupDoc.members[i].resources;
                
                    // for each resource
                    for (i2 = 0; i2 < userResourceList.length; i2++) {
                        resourceRef = userResourceList[i2];
                        if (typeof(resourceRef) != 'undefined' && resourceRef != null)
                            deleteResource(resourceRef);
                    }
                }
                // remove user element from group membership collection
                groupDoc.members.splice(i,1);
                break;
            }
        }
        // find and remove group element from user membership collection
        for (i = 0; i < userDoc.groupmbrship.length; i++) {
            if (userDoc.groupmbrship[i].id === groupDoc.id) {
                userDoc.groupmbrship.splice(i,1);
                break;
            }
        }

        // perform updates to user and group document
        var accept = collection.replaceDocument(groupDoc._self, groupDoc,
            function (err, docReplaced) {
                if (err) throw "Unable to update group document " + groupDoc.id + ", abort ";

                var accept2 = collection.replaceDocument(userDoc._self, userDoc,
                    function (err2, docReplaced2) {
                        if (err2) throw "Unable to update user document " + userDoc.id + ", abort"
                    });

                if (!accept2) throw "Unable to update user document " + userDoc.id + ", abort";
            });
        if (!accept) throw "Unable to update group document " + groupDoc.id + ", abort";
    }

    // flag the resource document as deleted
    function deleteResource(resourceRef) {
        var resourceQueryFilter = "SELECT * FROM r where r.id  = '" + resourceRef.id + "' AND r.type = 'resource'";

        // get the resource document
        var accept1 = collection.queryDocuments(collection.getSelfLink(), resourceQueryFilter, {},
            function (err1, documents1, responseOptions1) {
                if (err1) throw new Error("Error" + err1.message);
                if (documents1.length != 1) throw "Unable to find resource " + resourceRef.id;
                resourceDoc = documents1[0];

                // flag resource as deleted
                resourceDoc.deleted = true;

                // update document
                var accept2 = collection.replaceDocument(resourceDoc._self, resourceDoc,
                    function (err2, docReplaced2) {
                        if (err2) throw "Err: Unable to update resource document " + resourceRef.id + ", abort"
                    });
                if (!accept2) throw "Accept: Unable to update resource document " + resourceRef.id + ", abort";
                    return;
                });
        if (!accept1) throw "Unable to read resource document" + resourceRef.Id + ", abort ";
 
    }
}
