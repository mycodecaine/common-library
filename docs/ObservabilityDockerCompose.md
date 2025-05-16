# Observability Docker Compose

| Service         | Function                                                                 | Category        |
|----------------|--------------------------------------------------------------------------|-----------------|
| **loki**        | Collects, stores, and indexes logs for search and analysis              | Telemetry       |
| **prometheus**  | Scrapes, collects, and stores time-series metrics from targets          | Telemetry       |
| **jaeger**      | Collects and visualizes distributed traces across services              | Telemetry + Observability |
| **otel-collector** | Aggregates telemetry data (metrics, logs, traces) and exports to backends | Telemetry   |
| **grafana**     | Visualizes metrics, logs, and traces via dashboards and panels          | Observability   |


## Command

cd docker-compose/observability

docker-compose down

docker-compose up -d

## Validate Component

### 1. Loki
http://localhost:3100/ready

### 2. Loki - metrics
http://localhost:3100/metrics

### 3. Prometheus
http://localhost:9090/query

### 4. Grafana
http://localhost:3000/

username : admin / password : admin

### 5. Jaeger
http://localhost:16686/search