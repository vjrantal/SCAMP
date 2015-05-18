
Front End Module: Resource Retrieval for Dashboard

Use Case 1: Get user/student resources(Standard view)
Front-End Module Client: Dashboard
service path: /api/user/:userId/resources/all/
action: GET
Expected Response : {
   groups: [{
       groupName: {string},
       groupId: {string},
       unitsAllocated: {number},
       resources: [{
            resourceName: {string},
            resourceId: {string},
            state: {string},
            type: {string},
            unitsUsed: {number},
       }]
   }],
}

Use Case 2: Get group resources(Admin view), than an individual administers
Front-End Module Client: Dashboard
service path: /api/user/:userId/group/resources/all/
action: GET
Expected Response : {
   groups: [{
       groupName: {string},
       groupId: {string},
       totalUnitsAllocated: {number},
       unitsBudgeted: {number},
       totalUnitsUsed: {number},
       resources: [{
            resourceName: {string},
            resourceId: {string},
            type: {string},
            totalUnitsUsed: {number},
            unitsRemaining:  {number}
       }]
   }],
}

Use Case 3: Site Admin - Get all group admins
Front-End Module Client: Dashboard
service path: /api/user/:userId/group/resources/all/
action: GET
Expected Response : {
   users: [{
       userName: {string},
       userId: {string},
       unitsBudgeted: {number},
   }]
}

Use Case 4: Membership Mgr - Create New Group
Front-End Module Client: Membership Manager
service path: /api/resource/manager/groups/
action: Post
Expected Response : {
       groupName: {string},
       groupOwnerUserId: {string}
       userAdmins: [
                     {
                        userId{string}
                     }
                    ],
       perUserQuota: {number},
       expirationDate: {datetime},
       groupBudget: {number},
       defaultPerUserQuota: {number},
       templates[
                 {
                   templateId: {string}
                 }
                ],
       users: [
                {
                  userId: {string},
                  userQuotaOverrideUnits: {number}
                }
            ]
    }
