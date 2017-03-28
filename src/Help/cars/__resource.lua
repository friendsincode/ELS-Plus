
-- Manifest
resource_manifest_version '77731fab-63ca-442c-a67b-abc70f28dfa5'

local function vcf_loader()
	local vcf_files = {
		-- Add vehicle models
		-- here, separated by
		-- a comma after each
		-- line, except the last.
		-- Like this:
		-- 'POLICE',
		-- 'POLICE2',
		-- 'FBI'
	}
	for i = 1, #vcf_files do
		local car = vcf_files[i]:upper()
		files('stream/' .. car .. '/ELS/' .. car .. '.xml')
		ELSFMVCF('stream/' .. car .. '/ELS/' .. car .. '.xml')
	end
end

vcf_loader()