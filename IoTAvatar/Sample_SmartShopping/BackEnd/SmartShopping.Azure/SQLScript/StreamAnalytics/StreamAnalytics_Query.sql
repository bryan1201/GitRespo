SELECT
    [BeaconId],[TargetDeviceId],[SignalStrength],[Timestamp], System.timestamp as [CreatedTime]
INTO
    [OutputSQL]
FROM
    [InputIoTHub]