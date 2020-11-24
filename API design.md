AC1: As a user, I can add a company if its name no same to any existing company

**POST /companies**

Request body:

```json
{
    "companyName": ""
}
```

Response code: 201

Response body:

```json
{
    "companyId": "",
    "companyName": "",
    "employees": []
}
```

Header Location: /companies/{companyId}

---

AC2: As a user, I can obtain all company list

**GET /companies**

Response code: 200

Response body:

```json
[
    {
        "companyId": "",
        "companyName": "",
        "employees": []
    },
    ...
]
```

---

AC3: As a user, I can obtain an existing company

**GET /companies/{companyId}**

Response code: 200

Response body:

```json
{
    "companyId": "",
    "companyName": "",
    "employees": []
}
```

---

AC4: As a user, I can obtain X(page size) companies from index of Y(page index start from 1)

**GET /companies?limit=X&offset=Y**

Response code: 200

Response body:

```json
[
    {
        "companyId": "",
        "companyName": "",
        "employees": []
    },
    ...
]
```

---

AC5: As a user, I can update basic information of an existing company

**PATCH /companies/{companyId}**

Request body:

```json
{
    "CompanyName": ""
}
```

Response code: 200

Response body:

```json
{
    "companyId": "",
    "companyName": "",
    "employees": []
}
```

---

AC6: As a user, I can add an employee to a specific company

**POST /companies/{companyId}/employees**

Request body:

```json
{
    "name": "",
    "salary": ""
}
```

Response code: 201

Response body:

```json
{
    "employeeId": "",
    "name": "",
    "salary": ""
}
```

---

AC7: As a user, I can obtain list of all employee under a specific company

**GET /companies/{companyId}/employees**

Response code: 200

Response body:

```json
[
    {
        "employeeId": "",
        "name": "",
        "salary": ""
    },
    ...
]
```

---

AC8: As a user, I can update basic information of a specific employee under a specific company

**PATCH /companies/{companyId}/employees/{employeeId}**

Request body:

```json
{
    "name": "",
    "salary": ""
}
```

Response code: 200

Response body:

```json
{
    "employeeId": "",
    "name": "",
    "salary": ""
}
```

---

AC9: As a user, I can delete a specific employee under a specific company.

**DELETE /companies/{companyId}/employees/{employeeId}**

Response code: 204

---

AC10: As a user, I can delete a specific company, and all employees belong to this company should also be deleted

**DELETE /companies/{companyId}**

Response code: 204