# {{API_NAME}} API Reference

## Overview

<!-- Purpose of this API. Who consumes it and why. -->

## Base URL

```
{{BASE_URL}}
```

## Authentication

<!-- Auth method: Bearer token, API key, mTLS, etc. How to obtain credentials. -->

## Endpoints

### {{ENDPOINT_NAME}}

```
{{HTTP_METHOD}} {{PATH}}
```

**Description:** <!-- What this endpoint does. -->

**Auth required:** {{AUTH_REQUIRED}}

#### Request

**Headers**

| Header | Required | Description |
|---|---|---|
| `Content-Type` | Yes | `application/json` |

**Path parameters**

| Parameter | Type | Description |
|---|---|---|

**Query parameters**

| Parameter | Type | Required | Default | Description |
|---|---|---|---|---|

**Request body**

```json
{
  // example
}
```

#### Response

**200 OK**

```json
{
  // example
}
```

| Field | Type | Description |
|---|---|---|

#### Errors

| Status | Code | Description |
|---|---|---|
| 400 | `invalid_request` | |
| 401 | `unauthorized` | |
| 404 | `not_found` | |
| 500 | `server_error` | |

#### Example

```bash
curl -X {{HTTP_METHOD}} "{{BASE_URL}}{{PATH}}" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json"
```

---

<!-- Repeat the Endpoints section for each endpoint -->
