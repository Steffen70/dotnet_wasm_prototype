## Development Certificates

### Overview

Breakdown of the directory structure:

```text
cert/
├── root_ca.crt   # Public CA cert (trusted by browser/system)
├── root_ca.key   # Private CA key (kept secret)
├── localhost.crt # Signed certificate for localhost
├── localhost.key # Corresponding private key
├── localhost.pem # Combined cert+key for Kestrel (used in .NET)
```

**Note:** You can follow the steps below to create a self-signed certificate manually or just run `./create.sh` to automate the process.

### Installation

**Note:** Many browsers manage their own certificate stores, so you may need to install the Root CA certificate in your browser to trust the development certificates.

#### Install the Root CA Certificate (Ubuntu)

**1. Copy Certificate to Trusted Store Location**

```sh
sudo cp root_ca.pem /usr/local/share/ca-certificates/root_ca.crt
```

**2. Update Trusted Certificates**

```sh
sudo update-ca-certificates
```

#### Install the Root CA Certificate (Windows)

1. **Press `Win + R`**, type `mmc`, and press **Enter**.
2. In MMC, go to **File → Add/Remove Snap-in...**
3. Select **Certificates** and click **Add**.
4. Choose **Computer account**, then **Next → Finish → OK**.
5. In the left pane, expand **Certificates (Local Computer)** → **Trusted Root Certification Authorities** → **Certificates**.
6. **Right-click** on **Certificates**, then choose **All Tasks → Import...**
7. Click **Next**, **Browse** to your `root_ca.crt` file (select "All Files _._" if needed).
8. Make sure the certificate store is set to **Trusted Root Certification Authorities**.
9. Click **Next → Finish**.
10. You should see a success message. Close MMC and choose **No** when asked to save the console settings.

#### Brave (or Chrome):

1.  Open Brave and go to chrome://settings/security.
2.  Scroll down and click Manage certificates.
3.  Go to the Authorities tab.
4.  Click Import and select your `root_ca.crt` file.
5.  Ensure that the options to Trust this certificate for identifying websites are checked.

#### LibreWolf (or Firefox):

1.  Open LibreWolf and go to about:preferences#privacy.
2.  Scroll down to the Certificates section and click View Certificates.
3.  Go to the Authorities tab and click Import.
4.  Select your `root_ca.crt` and choose to Trust this CA to identify websites.

### Steps to create a self-signed certificate for localhost

#### 1. **Create a Root CA**

The Root CA will be trusted by your system and browser, and it will sign the development certificates (like `localhost.crt`).

**a. Generate a private key for the Root CA:**

```sh
openssl genrsa -out root_ca.key 2048
```

**b. Create a Root CA certificate:**

```sh
openssl req -x509 -new -nodes -key root_ca.key -sha256 -days 1024 -out root_ca.crt -config root_ca.conf
```

**Note:** During this process, you’ll be asked to provide information. Make sure to set a **Common Name (CN)** that reflects the purpose of the certificate, such as "Local Dev Root CA".

#### 2. **Create a Certificate for Localhost**

Now, you'll create a certificate for localhost signed by the Root CA.

**a. Generate a private key for localhost:**

```sh
openssl genrsa -out localhost.key 2048
```

**b. Create a certificate signing request (CSR) for localhost:**

```sh
openssl req -new -key localhost.key -out localhost.csr -config localhost.conf
```

**c. Sign the localhost certificate with the Root CA:**

```sh
openssl x509 -req -in localhost.csr -CA root_ca.crt -CAkey root_ca.key -CAcreateserial -out localhost.crt -days 500 -sha256 -extfile localhost.conf -extensions req_ext
```

**d. combine the private key and public key into a single `pem` file:**

```sh
cat localhost.crt localhost.key > localhost.pem
```

This signs the certificate with the Root CA, which allows it to be trusted as long as the Root CA is trusted.

---

Now you can install the Root CA certificate (`root_ca.crt`) as a trusted certificate authority on your system and browsers.
