"use strict";

const apiBaseUrl = "/api/planning";

const elements = {
    planningRequest:
        document.getElementById("planningRequest"),

    planningIdInput:
        document.getElementById("planningIdInput"),

    createPlanningButton:
        document.getElementById("createPlanningButton"),

    resetExampleButton:
        document.getElementById("resetExampleButton"),

    loadPlanningButton:
        document.getElementById("loadPlanningButton"),

    copyPlanningIdButton:
        document.getElementById("copyPlanningIdButton"),

    currentPlanningCard:
        document.getElementById("currentPlanningCard"),

    currentPlanningId:
        document.getElementById("currentPlanningId"),

    currentPlanningDate:
        document.getElementById("currentPlanningDate"),

    notification:
        document.getElementById("notification"),

    dashboardContent:
        document.getElementById("dashboardContent"),

    kpiGrid:
        document.getElementById("kpiGrid"),

    recommendationsGrid:
        document.getElementById("recommendationsGrid"),

    droneAnalysisTableBody:
        document.getElementById("droneAnalysisTableBody"),

    routesGrid:
        document.getElementById("routesGrid"),

    impossibleOrdersContainer:
        document.getElementById(
            "impossibleOrdersContainer"),

    loadingOverlay:
        document.getElementById("loadingOverlay")
};

let currentPlanningId = null;

const exampleRequest = {
    drones: [
        {
            id: "DRONE-01",
            capacityKg: 10,
            rangeKm: 30
        },
        {
            id: "DRONE-02",
            capacityKg: 20,
            rangeKm: 50
        },
        {
            id: "DRONE-03",
            capacityKg: 8,
            rangeKm: 20
        }
    ],
    orders: [
        {
            id: "ORDER-001",
            weightKg: 4,
            priority: "High",
            x: 3,
            y: 4
        },
        {
            id: "ORDER-002",
            weightKg: 6,
            priority: "Medium",
            x: -4,
            y: 3
        },
        {
            id: "ORDER-003",
            weightKg: 3,
            priority: "Low",
            x: 6,
            y: -2
        },
        {
            id: "ORDER-004",
            weightKg: 5,
            priority: "High",
            x: -2,
            y: -5
        },
        {
            id: "ORDER-005",
            weightKg: 7,
            priority: "Medium",
            x: 8,
            y: 4
        }
    ]
};

initialize();

function initialize() {
    resetExampleRequest();

    elements.createPlanningButton.addEventListener(
        "click",
        createPlanning);

    elements.resetExampleButton.addEventListener(
        "click",
        resetExampleRequest);

    elements.loadPlanningButton.addEventListener(
        "click",
        loadPlanningFromInput);

    elements.copyPlanningIdButton.addEventListener(
        "click",
        copyPlanningId);

    elements.planningIdInput.addEventListener(
        "keydown",
        event => {
            if (event.key === "Enter") {
                loadPlanningFromInput();
            }
        });

    const planningIdFromUrl =
        new URLSearchParams(window.location.search)
            .get("planningId");

    if (planningIdFromUrl) {
        elements.planningIdInput.value =
            planningIdFromUrl;

        loadPlanning(planningIdFromUrl);
    }
}

function resetExampleRequest() {
    elements.planningRequest.value =
        JSON.stringify(exampleRequest, null, 2);
}

async function createPlanning() {
    clearNotification();

    let request;

    try {
        request = JSON.parse(
            elements.planningRequest.value);
    } catch {
        showNotification(
            "The planning request is not valid JSON.",
            "error");

        return;
    }

    setLoading(true);

    try {
        const response = await fetch(apiBaseUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(request)
        });

        if (!response.ok) {
            throw await createApiError(
                response,
                "Unable to create the planning.");
        }

        const createdPlanning =
            await response.json();

        const planningId =
            createdPlanning.planningId;

        if (!planningId) {
            throw new Error(
                "The API did not return a planning ID.");
        }

        elements.planningIdInput.value =
            planningId;

        showNotification(
            "Planning created successfully.",
            "success");

        await loadPlanning(
            planningId,
            createdPlanning.createdAtUtc);
    } catch (error) {
        showNotification(
            error.message,
            "error");
    } finally {
        setLoading(false);
    }
}

async function loadPlanningFromInput() {
    const planningId =
        elements.planningIdInput.value.trim();

    if (!planningId) {
        showNotification(
            "Enter a planning ID before loading the dashboard.",
            "information");

        return;
    }

    await loadPlanning(planningId);
}

async function loadPlanning(
    planningId,
    knownCreatedAtUtc = null) {

    clearNotification();
    setLoading(true);

    try {
        const [
            planningResponse,
            routesResponse,
            dronesResponse,
            analysisResponse
        ] = await Promise.all([
            fetch(`${apiBaseUrl}/${planningId}`),

            fetch(
                `${apiBaseUrl}/${planningId}/routes`),

            fetch(
                `${apiBaseUrl}/${planningId}/drones`),

            fetch(
                `${apiBaseUrl}/${planningId}/fleet-analysis`)
        ]);

        const responses = [
            planningResponse,
            routesResponse,
            dronesResponse,
            analysisResponse
        ];

        const notFound =
            responses.some(
                response =>
                    response.status === 404);

        if (notFound) {
            throw new Error(
                "Planning not found. Verify the provided ID.");
        }

        const failedResponse =
            responses.find(
                response => !response.ok);

        if (failedResponse) {
            throw await createApiError(
                failedResponse,
                "Unable to load planning data.");
        }

        const [
            planning,
            routes,
            drones,
            analysis
        ] = await Promise.all([
            planningResponse.json(),
            routesResponse.json(),
            dronesResponse.json(),
            analysisResponse.json()
        ]);

        currentPlanningId = planningId;

        renderDashboard({
            planningId,
            createdAtUtc: knownCreatedAtUtc,
            planning,
            routes,
            drones,
            analysis
        });

        updateBrowserUrl(planningId);

        showNotification(
            "Planning dashboard loaded successfully.",
            "success");
    } catch (error) {
        showNotification(
            error.message,
            "error");
    } finally {
        setLoading(false);
    }
}

function renderDashboard(data) {
    renderCurrentPlanning(
        data.planningId,
        data.createdAtUtc);

    renderKpis(
        data.analysis,
        data.drones);

    renderRecommendations(
        data.analysis.recommendations ?? []);

    renderDroneAnalysis(
        data.analysis.drones ?? []);

    renderRoutes(
        data.routes ?? []);

    renderImpossibleOrders(
        data.planning.impossibleOrders ?? []);

    elements.dashboardContent.classList.remove(
        "hidden");

    elements.copyPlanningIdButton.disabled = false;
}

function renderCurrentPlanning(
    planningId,
    createdAtUtc) {

    elements.currentPlanningId.textContent =
        planningId;

    elements.currentPlanningDate.textContent =
        createdAtUtc
            ? `Created ${formatDate(createdAtUtc)}`
            : "Loaded from in-memory storage";

    elements.currentPlanningCard.classList.remove(
        "hidden");
}

function renderKpis(analysis, drones) {
    const unusedDrones =
        drones.filter(drone => !drone.wasUsed).length;

    const kpis = [
        {
            label: "Total trips",
            value: formatNumber(
                analysis.totalTrips),
            detail:
                `${formatNumber(
                    analysis.deliveredOrders
                )} delivered orders`
        },
        {
            label: "Delivered weight",
            value:
                `${formatNumber(
                    analysis.totalDeliveredWeightKg,
                    2
                )} kg`,
            detail:
                `${formatNumber(
                    analysis.fleetEfficiencyKgPerKm,
                    2
                )} kg/km efficiency`
        },
        {
            label: "Total distance",
            value:
                `${formatNumber(
                    analysis.totalDistanceKm,
                    2
                )} km`,
            detail:
                `${formatNumber(
                    analysis.estimatedTotalTimeMinutes,
                    1
                )} estimated minutes`
        },
        {
            label: "Fleet participation",
            value:
                `${formatNumber(
                    analysis.fleetParticipationPercentage,
                    1
                )}%`,
            detail:
                `${analysis.usedDrones} of ` +
                `${analysis.totalDrones} drones used`
        },
        {
            label: "Average load",
            value:
                `${formatNumber(
                    analysis.averageLoadFactorPercentage,
                    1
                )}%`,
            detail: "Average across all trips"
        },
        {
            label: "Unused drones",
            value: formatNumber(unusedDrones),
            detail:
                `${formatNumber(
                    analysis.impossibleOrders
                )} impossible orders`
        }
    ];

    elements.kpiGrid.innerHTML =
        kpis.map(createKpiCard).join("");
}

function createKpiCard(kpi) {
    return `
        <article class="kpi-card">
            <span class="kpi-card__label">
                ${escapeHtml(kpi.label)}
            </span>

            <strong class="kpi-card__value">
                ${escapeHtml(kpi.value)}
            </strong>

            <span class="kpi-card__detail">
                ${escapeHtml(kpi.detail)}
            </span>
        </article>
    `;
}

function renderRecommendations(recommendations) {
    if (!recommendations.length) {
        elements.recommendationsGrid.innerHTML = `
            <div class="empty-state">
                No recommendations are available.
            </div>
        `;

        return;
    }

    elements.recommendationsGrid.innerHTML =
        recommendations
            .map(createRecommendationCard)
            .join("");
}

function createRecommendationCard(
    recommendation) {

    const severity =
        normalizeSeverity(
            recommendation.severity);

    const suggestionTags = [];

    if (
        recommendation
            .suggestedMinimumCapacityKg !== null
        && recommendation
            .suggestedMinimumCapacityKg !== undefined
    ) {
        suggestionTags.push(
            `Minimum capacity: ` +
            `${formatNumber(
                recommendation
                    .suggestedMinimumCapacityKg,
                2
            )} kg`);
    }

    if (
        recommendation
            .suggestedMinimumRangeKm !== null
        && recommendation
            .suggestedMinimumRangeKm !== undefined
    ) {
        suggestionTags.push(
            `Minimum range: ` +
            `${formatNumber(
                recommendation
                    .suggestedMinimumRangeKm,
                2
            )} km`);
    }

    const metadata = [
        recommendation.type,
        recommendation.severity,
        ...suggestionTags
    ];

    return `
        <article
            class="
                recommendation-card
                recommendation-card--${severity}
            ">

            <div class="recommendation-card__header">
                <h3>
                    ${escapeHtml(
        recommendation.title)}
                </h3>
            </div>

            <p>
                ${escapeHtml(
            recommendation.description)}
            </p>

            <div class="recommendation-card__metadata">
                ${metadata
            .map(value => `
                        <span class="tag">
                            ${escapeHtml(value)}
                        </span>
                    `)
            .join("")}
            </div>
        </article>
    `;
}

function renderDroneAnalysis(drones) {
    if (!drones.length) {
        elements.droneAnalysisTableBody.innerHTML = `
            <tr>
                <td colspan="11">
                    No drone analysis is available.
                </td>
            </tr>
        `;

        return;
    }

    elements.droneAnalysisTableBody.innerHTML =
        drones.map(drone => `
            <tr>
                <td>
                    <strong>
                        ${escapeHtml(drone.droneId)}
                    </strong>
                </td>

                <td>
                    <span class="
                        status-badge
                        ${drone.wasUsed
                ? "status-badge--active"
                : "status-badge--inactive"
            }
                    ">
                        ${drone.wasUsed
                ? "Used"
                : "Unused"
            }
                    </span>
                </td>

                <td>
                    ${formatNumber(drone.tripCount)}
                </td>

                <td>
                    ${formatNumber(
                drone.deliveredOrders)}
                </td>

                <td>
                    ${formatNumber(
                    drone.deliveredWeightKg,
                    2
                )} kg
                </td>

                <td>
                    ${formatNumber(
                    drone.distanceKm,
                    2
                )} km
                </td>

                <td>
                    ${formatNumber(
                    drone.efficiencyKgPerKm,
                    2
                )} kg/km
                </td>

                <td>
                    ${formatNumber(
                    drone
                        .averageLoadFactorPercentage,
                    1
                )}%
                </td>

                <td>
                    ${formatNumber(
                    drone
                        .averageBatteryUsagePerTripPercentage,
                    1
                )}%
                </td>

                <td>
                    ${formatNumber(
                    drone
                        .maximumBatteryUsagePerTripPercentage,
                    1
                )}%
                </td>

                <td>
                    ${formatNumber(
                    drone.estimatedTimeMinutes,
                    1
                )} min
                </td>
            </tr>
        `).join("");
}

function renderRoutes(routes) {
    if (!routes.length) {
        elements.routesGrid.innerHTML = `
            <div class="empty-state">
                No routes were generated for this planning.
            </div>
        `;

        return;
    }

    elements.routesGrid.innerHTML =
        routes
            .map(createRouteCard)
            .join("");
}

function createRouteCard(route) {
    const svg =
        createRouteSvg(route);

    const stops =
        route.stops ?? [];

    return `
        <article class="route-card">
            <header class="route-card__header">
                <div>
                    <h3>
                        Trip ${formatNumber(
        route.tripSequence)}
                    </h3>

                    <p>
                        Drone
                        <strong>
                            ${escapeHtml(route.droneId)}
                        </strong>
                    </p>
                </div>

                <div class="route-card__metrics">
                    <div>
                        ${formatNumber(
            route.totalWeightKg,
            2
        )} kg
                    </div>

                    <div>
                        ${formatNumber(
            route.totalDistanceKm,
            2
        )} km
                    </div>
                </div>
            </header>

            ${svg}

            <div class="route-card__stops">
                ${stops.length
            ? stops.map(stop => `
                        <div class="route-stop">
                            <span
                                class="route-stop__sequence">
                                ${formatNumber(
                stop.sequence)}
                            </span>

                            <strong>
                                ${escapeHtml(
                    stop.orderId)}
                            </strong>

                            <span>
                                (${formatNumber(
                        stop.x,
                        2
                    )},
                                ${formatNumber(
                        stop.y,
                        2
                    )})
                            </span>
                        </div>
                    `).join("")
            : `
                        <span>
                            No delivery stops.
                        </span>
                    `}
            </div>
        </article>
    `;
}

function createRouteSvg(route) {
    const width = 560;
    const height = 350;
    const padding = 45;

    const stops = route.stops ?? [];

    const coordinates = [
        { x: 0, y: 0 },
        ...stops.map(stop => ({
            x: Number(stop.x),
            y: Number(stop.y)
        })),
        { x: 0, y: 0 }
    ];

    const allX = coordinates.map(point => point.x);
    const allY = coordinates.map(point => point.y);

    const minimumX = Math.min(...allX, -1);
    const maximumX = Math.max(...allX, 1);
    const minimumY = Math.min(...allY, -1);
    const maximumY = Math.max(...allY, 1);

    const xRange =
        Math.max(maximumX - minimumX, 2);

    const yRange =
        Math.max(maximumY - minimumY, 2);

    const scaleX =
        (width - padding * 2) / xRange;

    const scaleY =
        (height - padding * 2) / yRange;

    const scale =
        Math.min(scaleX, scaleY);

    const centerX =
        (minimumX + maximumX) / 2;

    const centerY =
        (minimumY + maximumY) / 2;

    const project = point => ({
        x:
            width / 2 +
            (point.x - centerX) * scale,

        y:
            height / 2 -
            (point.y - centerY) * scale
    });

    const projectedCoordinates =
        coordinates.map(project);

    const polylinePoints =
        projectedCoordinates
            .map(point =>
                `${point.x},${point.y}`)
            .join(" ");

    const projectedDepot =
        project({ x: 0, y: 0 });

    const projectedStops =
        stops.map(stop => ({
            ...stop,
            projected: project({
                x: Number(stop.x),
                y: Number(stop.y)
            })
        }));

    const xAxisY =
        projectedDepot.y;

    const yAxisX =
        projectedDepot.x;

    return `
        <svg
            class="route-map"
            viewBox="0 0 ${width} ${height}"
            role="img"
            aria-label="
                Route for ${escapeHtml(route.droneId)}
            ">

            <line
                x1="${padding}"
                y1="${xAxisY}"
                x2="${width - padding}"
                y2="${xAxisY}"
                stroke="#cbd5e1"
                stroke-width="1"
                stroke-dasharray="5 5">
            </line>

            <line
                x1="${yAxisX}"
                y1="${padding}"
                x2="${yAxisX}"
                y2="${height - padding}"
                stroke="#cbd5e1"
                stroke-width="1"
                stroke-dasharray="5 5">
            </line>

            <polyline
                points="${polylinePoints}"
                fill="none"
                stroke="#1d4ed8"
                stroke-width="4"
                stroke-linecap="round"
                stroke-linejoin="round">
            </polyline>

            <circle
                cx="${projectedDepot.x}"
                cy="${projectedDepot.y}"
                r="10"
                fill="#17212b"
                stroke="#ffffff"
                stroke-width="4">
            </circle>

            <text
                x="${projectedDepot.x + 14}"
                y="${projectedDepot.y - 13}"
                fill="#17212b"
                font-size="13"
                font-weight="700">
                Depot
            </text>

           ${projectedStops.map(stop => {
               const isNearRightEdge =
                   stop.projected.x > width - 120;

               const isNearTopEdge =
                   stop.projected.y < 35;

               const labelX = isNearRightEdge
                   ? stop.projected.x - 15
                   : stop.projected.x + 15;

               const labelY = isNearTopEdge
                   ? stop.projected.y + 28
                   : stop.projected.y - 13;

               const textAnchor = isNearRightEdge
                   ? "end"
                   : "start";

               return `
            <circle
                cx="${stop.projected.x}"
                cy="${stop.projected.y}"
                r="11"
                fill="#1d4ed8"
                stroke="#ffffff"
                stroke-width="4">
            </circle>

            <text
                x="${stop.projected.x}"
                y="${stop.projected.y + 4}"
                text-anchor="middle"
                fill="#ffffff"
                font-size="10"
                font-weight="800">
                ${stop.sequence}
            </text>

            <text
                x="${labelX}"
                y="${labelY}"
                text-anchor="${textAnchor}"
                fill="#334155"
                font-size="12"
                font-weight="700">
                ${escapeHtml(stop.orderId)}
            </text>
        `;
               }).join("")}
        </svg>
    `;
}

function renderImpossibleOrders(
    impossibleOrders) {

    if (!impossibleOrders.length) {
        elements.impossibleOrdersContainer.className =
            "empty-state";

        elements.impossibleOrdersContainer.innerHTML =
            "No impossible orders were identified.";

        return;
    }

    elements.impossibleOrdersContainer.className =
        "impossible-orders-list";

    elements.impossibleOrdersContainer.innerHTML =
        impossibleOrders.map(order => `
            <article class="impossible-order">
                <strong>
                    ${escapeHtml(order.orderId)}
                </strong>

                <span>
                    ${escapeHtml(order.reason)}
                </span>
            </article>
        `).join("");
}

async function copyPlanningId() {
    if (!currentPlanningId) {
        return;
    }

    try {
        await navigator.clipboard.writeText(
            currentPlanningId);

        showNotification(
            "Planning ID copied to the clipboard.",
            "success");
    } catch {
        showNotification(
            "The browser could not copy the planning ID.",
            "error");
    }
}

function updateBrowserUrl(planningId) {
    const url = new URL(window.location.href);

    url.searchParams.set(
        "planningId",
        planningId);

    window.history.replaceState(
        {},
        "",
        url);
}

async function createApiError(
    response,
    fallbackMessage) {

    const contentType =
        response.headers.get("content-type") ?? "";

    if (contentType.includes("application/json")) {
        const body = await response.json();

        const message =
            body.detail
            ?? body.title
            ?? extractValidationErrors(body)
            ?? fallbackMessage;

        return new Error(message);
    }

    const text = await response.text();

    return new Error(
        text.trim() || fallbackMessage);
}

function extractValidationErrors(body) {
    if (!body?.errors) {
        return null;
    }

    return Object.values(body.errors)
        .flat()
        .join(" ");
}

function setLoading(isLoading) {
    elements.loadingOverlay.classList.toggle(
        "hidden",
        !isLoading);

    elements.createPlanningButton.disabled =
        isLoading;

    elements.loadPlanningButton.disabled =
        isLoading;
}

function showNotification(
    message,
    type = "information") {

    elements.notification.textContent = message;

    elements.notification.className =
        `notification notification--${type}`;

    window.clearTimeout(
        showNotification.timeoutId);

    showNotification.timeoutId =
        window.setTimeout(
            clearNotification,
            6000);
}

function clearNotification() {
    elements.notification.textContent = "";

    elements.notification.className =
        "notification hidden";
}

function normalizeSeverity(severity) {
    const normalized =
        String(severity ?? "information")
            .toLowerCase();

    const supportedSeverities = [
        "success",
        "information",
        "warning",
        "critical"
    ];

    return supportedSeverities.includes(normalized)
        ? normalized
        : "information";
}

function formatNumber(
    value,
    maximumFractionDigits = 0) {

    const number = Number(value ?? 0);

    return new Intl.NumberFormat(
        "en-US",
        {
            minimumFractionDigits: 0,
            maximumFractionDigits
        })
        .format(number);
}

function formatDate(value) {
    const date = new Date(value);

    if (Number.isNaN(date.getTime())) {
        return value;
    }

    return new Intl.DateTimeFormat(
        "en-US",
        {
            dateStyle: "medium",
            timeStyle: "short"
        })
        .format(date);
}

function escapeHtml(value) {
    return String(value ?? "")
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll("\"", "&quot;")
        .replaceAll("'", "&#039;");
}