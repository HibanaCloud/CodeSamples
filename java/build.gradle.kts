plugins {
    java
    application
}

group = "com.hibana"
version = "1.0.0"

java {
    sourceCompatibility = JavaVersion.VERSION_11
    targetCompatibility = JavaVersion.VERSION_11
}

repositories {
    mavenCentral()
}

dependencies {
    // OpenAI Java SDK
    implementation("com.openai:openai-java:4.7.1")

    // OkHttp for balance endpoint (custom HTTP calls)
    implementation("com.squareup.okhttp3:okhttp:4.12.0")

    // JSON parsing
    implementation("com.google.code.gson:gson:2.10.1")
}

tasks.withType<JavaCompile> {
    options.encoding = "UTF-8"
}

application {
    mainClass.set("com.hibana.samples.Example01_SimpleChat")
}
